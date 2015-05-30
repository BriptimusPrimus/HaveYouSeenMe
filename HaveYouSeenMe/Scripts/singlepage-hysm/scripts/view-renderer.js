define(
	function(){

		//must receive name of view and container jQuery object
		function renderView(view, container, model){				

			//Get the HTML from the template   in the script tag​
			var viewScript = $("#"+view);
			var theTemplateScript = viewScript.html(); 

   			//Compile the template​
	    	var theTemplate = Handlebars.compile(theTemplateScript); 
	    	container.html(theTemplate(model));	
	    	container.append('<script src="/Scripts/jquery.unobtrusive-ajax.js"></script>');	    	
	    	container.append('<script src="/Scripts/jquery.validate.js"></script>');		
	    	container.append('<script src="/Scripts/jquery.validate.unobtrusive.js"></script>');
		}		

		return{
			renderTemplate: function(view, container, model){
				renderView(view, container, model);		
			}
		}	
	}
);