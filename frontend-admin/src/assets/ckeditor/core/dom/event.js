
CKEDITOR.dom.event = function( domEvent ) {
	/**
	 * The native DOM event object represented by this class instance.
	 *
	 * @readonly
	 */
	this.$ = domEvent;
};

CKEDITOR.dom.event.prototype = {
	/**
	 * Gets the key code associated to the event.
	 *
	 *		alert( event.getKey() ); // '65' if 'a' has been pressed
	 *
	 * @returns {Number} The key code.
	 */
	getKey: function() {
		return this.$.keyCode || this.$.which;
	},


	getKeystroke: function() {
		var keystroke = this.getKey();

		if ( this.$.ctrlKey || this.$.metaKey )
			keystroke += CKEDITOR.CTRL;

		if ( this.$.shiftKey )
			keystroke += CKEDITOR.SHIFT;

		if ( this.$.altKey )
			keystroke += CKEDITOR.ALT;

		return keystroke;
	},


	preventDefault: function( stopPropagation ) {
		var $ = this.$;
		if ( $.preventDefault )
			$.preventDefault();
		else
			$.returnValue = false;

		if ( stopPropagation )
			this.stopPropagation();
	},

	/**
	 * Stops this event propagation in the event chain.
	 */
	stopPropagation: function() {
		var $ = this.$;
		if ( $.stopPropagation )
			$.stopPropagation();
		else
			$.cancelBubble = true;
	},


	getTarget: function() {
		var rawNode = this.$.target || this.$.srcElement;
		return rawNode ? new CKEDITOR.dom.node( rawNode ) : null;
	},


	getPhase: function() {
		return this.$.eventPhase || 2;
	},

	/**
	 * Retrieves the coordinates of the mouse pointer relative to the top-left
	 * corner of the document, in mouse related event.
	 *
	 *		element.on( 'mousemouse', function( ev ) {
	 *			var pageOffset = ev.data.getPageOffset();
	 *			alert( pageOffset.x );			// page offset X
	 *			alert( pageOffset.y );			// page offset Y
	 *     } );
	 *
	 * @returns {Object} The object contains the position.
	 * @returns {Number} return.x
	 * @returns {Number} return.y
	 */
	getPageOffset: function() {
		var doc = this.getTarget().getDocument().$;
		var pageX = this.$.pageX || this.$.clientX + ( doc.documentElement.scrollLeft || doc.body.scrollLeft );
		var pageY = this.$.pageY || this.$.clientY + ( doc.documentElement.scrollTop || doc.body.scrollTop );
		return { x: pageX, y: pageY };
	}
};

// For the followind constants, we need to go over the Unicode boundaries
// (0x10FFFF) to avoid collision.


CKEDITOR.CTRL = 0x110000;


CKEDITOR.SHIFT = 0x220000;

CKEDITOR.ALT = 0x440000;

CKEDITOR.EVENT_PHASE_CAPTURING = 1;


CKEDITOR.EVENT_PHASE_AT_TARGET = 2;


CKEDITOR.EVENT_PHASE_BUBBLING = 3;
