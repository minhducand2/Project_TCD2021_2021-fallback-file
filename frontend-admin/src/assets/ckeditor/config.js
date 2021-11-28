/**
 * @license Copyright (c) 2003-2020, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function( config ) {
	
	// %REMOVE_START%
	// The configuration options below are needed when running CKEditor from source files.
	config.plugins = 'dialogui,dialog,about,a11yhelp,dialogadvtab,basicstyles,bidi,blockquote,notification,button,toolbar,clipboard,panelbutton,panel,floatpanel,colorbutton,templates,copyformatting,div,editorplaceholder,resize,elementspath,enterkey,entities,exportpdf,popup,filetools,filebrowser,find,fakeobjects,flash,floatingspace,listblock,richcombo,font,forms,format,horizontalrule,htmlwriter,iframe,wysiwygarea,image,indent,indentblock,indentlist,smiley,justify,menu,menubutton,language,link,list,contextmenu,liststyle,magicline,maximize,newpage,pagebreak,pastetext,pastetools,pastefromgdocs,pastefromword,preview,print,removeformat,save,selectall,showblocks,showborders,sourcearea,specialchar,scayt,stylescombo,tab,table,tabletools,tableselection,undo,lineutils,widgetselection,widget,notificationaggregator,uploadwidget,uploadimage,wsc,base64image,imageuploader,pastebase64,imageresizerowandcolumn,pastefromexcel,textwatcher,autocomplete,tableresizerowandcolumn,tableresize';
	config.skin = 'office2013'; 
	config.font_names = 'Arial;Times New Roman;Verdana;Roboto Slab;Open Sans;';
	config.font_defaultLabel = 'Roboto Slab';
	config.fontSize_defaultLabel = '18'
	// %REMOVE_END%

	// Define changes to default configuration here. For example:
	// config.language = 'fr';
	// config.uiColor = '#AADC6E';
};
