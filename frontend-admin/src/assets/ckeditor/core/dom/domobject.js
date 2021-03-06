
CKEDITOR.dom.domObject = function( nativeDomObject ) {
	if ( nativeDomObject ) {

		this.$ = nativeDomObject;
	}
};

CKEDITOR.dom.domObject.prototype = ( function() {
	// Do not define other local variables here. We want to keep the native
	// listener closures as clean as possible.

	var getNativeListener = function( domObject, eventName ) {
			return function( domEvent ) {
				// In FF, when reloading the page with the editor focused, it may
				// throw an error because the CKEDITOR global is not anymore
				// available. So, we check it here first. (https://dev.ckeditor.com/ticket/2923)
				if ( typeof CKEDITOR != 'undefined' )
					domObject.fire( eventName, new CKEDITOR.dom.event( domEvent ) );
			};
		};

	return {

		getPrivate: function() {
			var priv;

			// Get the main private object from the custom data. Create it if not defined.
			if ( !( priv = this.getCustomData( '_' ) ) )
				this.setCustomData( '_', ( priv = {} ) );

			return priv;
		},

		// Docs inherited from event.
		on: function( eventName ) {
			// We customize the "on" function here. The basic idea is that we'll have
			// only one listener for a native event, which will then call all listeners
			// set to the event.

			// Get the listeners holder object.
			var nativeListeners = this.getCustomData( '_cke_nativeListeners' );

			if ( !nativeListeners ) {
				nativeListeners = {};
				this.setCustomData( '_cke_nativeListeners', nativeListeners );
			}

			// Check if we have a listener for that event.
			if ( !nativeListeners[ eventName ] ) {
				var listener = nativeListeners[ eventName ] = getNativeListener( this, eventName );

				if ( this.$.addEventListener )
					this.$.addEventListener( eventName, listener, !!CKEDITOR.event.useCapture );
				else if ( this.$.attachEvent )
					this.$.attachEvent( 'on' + eventName, listener );
			}

			// Call the original implementation.
			return CKEDITOR.event.prototype.on.apply( this, arguments );
		},

		// Docs inherited from event.
		removeListener: function( eventName ) {
			// Call the original implementation.
			CKEDITOR.event.prototype.removeListener.apply( this, arguments );

			// If we don't have listeners for this event, clean the DOM up.
			if ( !this.hasListeners( eventName ) ) {
				var nativeListeners = this.getCustomData( '_cke_nativeListeners' );
				var listener = nativeListeners && nativeListeners[ eventName ];
				if ( listener ) {
					if ( this.$.removeEventListener )
						this.$.removeEventListener( eventName, listener, false );
					else if ( this.$.detachEvent )
						this.$.detachEvent( 'on' + eventName, listener );

					delete nativeListeners[ eventName ];
				}
			}
		},

		/**
		 * Removes any listener set on this object.
		 *
		 * To avoid memory leaks we must assure that there are no
		 * references left after the object is no longer needed.
		 */
		removeAllListeners: function() {
			try {
				var nativeListeners = this.getCustomData( '_cke_nativeListeners' );
				for ( var eventName in nativeListeners ) {
					var listener = nativeListeners[ eventName ];
					if ( this.$.detachEvent ) {
						this.$.detachEvent( 'on' + eventName, listener );
					} else if ( this.$.removeEventListener ) {
						this.$.removeEventListener( eventName, listener, false );
					}

					delete nativeListeners[ eventName ];
				}
			// Catch Edge `Permission denied` error which occurs randomly. Since the error is quite
			// random, catching allows to continue the code execution and cleanup (#3419).
			} catch ( error ) {
				if ( !CKEDITOR.env.edge || error.number !== -2146828218 ) {
					throw( error );
				}
			}

			// Remove events from events object so fire() method will not call
			// listeners (https://dev.ckeditor.com/ticket/11400).
			CKEDITOR.event.prototype.removeAllListeners.call( this );
		}
	};
} )();

( function( domObjectProto ) {
	var customData = {};

	CKEDITOR.on( 'reset', function() {
		customData = {};
	} );

	/**
	 * Determines whether the specified object is equal to the current object.
	 *
	 *		var doc = new CKEDITOR.dom.document( document );
	 *		alert( doc.equals( CKEDITOR.document ) );	// true
	 *		alert( doc == CKEDITOR.document );			// false
	 *
	 * @param {Object} object The object to compare with the current object.
	 * @returns {Boolean} `true` if the object is equal.
	 */
	domObjectProto.equals = function( object ) {
		// Try/Catch to avoid IE permission error when object is from different document.
		try {
			return ( object && object.$ === this.$ );
		} catch ( er ) {
			return false;
		}
	};


	domObjectProto.setCustomData = function( key, value ) {
		var expandoNumber = this.getUniqueId(),
			dataSlot = customData[ expandoNumber ] || ( customData[ expandoNumber ] = {} );

		dataSlot[ key ] = value;

		return this;
	};


	domObjectProto.getCustomData = function( key ) {
		var expandoNumber = this.$[ 'data-cke-expando' ],
			dataSlot = expandoNumber && customData[ expandoNumber ];

		return ( dataSlot && key in dataSlot ) ? dataSlot[ key ] : null;
	};

	/**
	 * Removes the value in the data slot under the given `key`.
	 *
	 * @param {String} key
	 * @returns {Object} Removed value or `null` if not found.
	 */
	domObjectProto.removeCustomData = function( key ) {
		var expandoNumber = this.$[ 'data-cke-expando' ],
			dataSlot = expandoNumber && customData[ expandoNumber ],
			retval, hadKey;

		if ( dataSlot ) {
			retval = dataSlot[ key ];
			hadKey = key in dataSlot;
			delete dataSlot[ key ];
		}

		return hadKey ? retval : null;
	};


	domObjectProto.clearCustomData = function() {
		// Clear all event listeners
		this.removeAllListeners();

		var expandoNumber = this.getUniqueId();
		expandoNumber && delete customData[ expandoNumber ];
	};

	domObjectProto.getUniqueId = function() {
		return this.$[ 'data-cke-expando' ] || ( this.$[ 'data-cke-expando' ] = CKEDITOR.tools.getNextNumber() );
	};

	// Implement CKEDITOR.event.
	CKEDITOR.event.implementOn( domObjectProto );

} )( CKEDITOR.dom.domObject.prototype );
