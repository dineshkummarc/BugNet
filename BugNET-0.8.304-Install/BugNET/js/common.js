function ToggleElement(sender)
{
	visible = (document.getElementById(sender).style.display == 'none') ? 'block' : 'none';
		document.getElementById(sender).style.display = visible;
}
function TogglePanel(panel, image){
    ToggleElement(panel);
    if(document.images[image].src.search("plus.gif") != -1){
        document.images[image].src="../../images/minus.gif"
     }
    else{
        document.images[image].src="../../images/plus.gif"
    }
}
function ToggleFields(){
		var linkText = document.getElementById('ToggleFields').innerHTML;
		document.getElementById('ToggleFields').innerHTML= (linkText == 'Remove optional fields?') ? 'Display optional fields?' : 'Remove optional fields?';
		
		var tmp = document.getElementsByTagName('div');
		for (var i=0;i<tmp.length;i++)
		{
			if(tmp[i].className == 'optional' || tmp[i].className == 'optional wide')
			{
				tmp[i].style.display = (tmp[i].style.display == 'none') ? 'block' : 'none';
			}
		}
		//SetFTBContainerLocation()
		return false;
}
function AttachmentChanged(sender){
	ToggleElement('Attachment');
	ValidatorEnable(document.getElementById('rvAttachedFile'),sender.checked);
	ValidatorEnable(document.getElementById('rvAttachmentDesc'),sender.checked);
}

function getposOffset(what, offsettype){
  var totaloffset=(offsettype=="left")? what.offsetLeft : what.offsetTop;
  var parentEl=what.offsetParent;
  while (parentEl!=null){
    totaloffset=(offsettype=="left")? totaloffset+parentEl.offsetLeft : totaloffset+parentEl.offsetTop;
    parentEl=parentEl.offsetParent;
  }
  return totaloffset;
}


/* Client-side access to querystring name=value pairs
	Version 1.2.3
	22 Jun 2005
	Adam Vandenberg
*/
function Querystring(qs) { // optionally pass a querystring to parse
	this.params = new Object()
	this.get=Querystring_get
	
	if (qs == null)
		qs=location.search.substring(1,location.search.length)

	if (qs.length == 0) return

// Turn <plus> back to <space>
// See: http://www.w3.org/TR/REC-html40/interact/forms.html#h-17.13.4.1
	qs = qs.replace(/\+/g, ' ')
	var args = qs.split('&') // parse out name/value pairs separated via &
	
// split out each name=value pair
	for (var i=0;i<args.length;i++) {
		var value;
		var pair = args[i].split('=')
		var name = unescape(pair[0])

		if (pair.length == 2)
			value = unescape(pair[1])
		else
			value = name
		
		this.params[name] = value
	}
}

function Querystring_get(key, default_) {
	// This silly looking line changes UNDEFINED to NULL
	if (default_ == null) default_ = null;
	
	var value=this.params[key]
	if (value==null) value=default_;
	
	return value
}
