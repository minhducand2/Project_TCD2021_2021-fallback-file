
CKEDITOR.dom.comment = function( comment, ownerDocument ) {
	if ( typeof comment == 'string' )
		comment = ( ownerDocument ? ownerDocument.$ : document ).createComment( comment );

	CKEDITOR.dom.domObject.call( this, comment );
};

CKEDITOR.dom.comment.prototype = new CKEDITOR.dom.node();

CKEDITOR.tools.extend( CKEDITOR.dom.comment.prototype, {
	/**
	 * The node type. This is a constant value set to {@link CKEDITOR#NODE_COMMENT}.
	 *
	 * @readonly
	 * @property {Number} [=CKEDITOR.NODE_COMMENT]
	 */
	type: CKEDITOR.NODE_COMMENT,

	/**
	 * Gets the outer HTML of this comment.
	 *
	 * @returns {String} The HTML `<!-- comment value -->`.
	 */
	getOuterHtml: function() {
		return '<!--' + this.$.nodeValue + '-->';
	}
} );
