define(["view-renderer"],
	function(renderer){
		var model,
			view = "pet-details-view",
			mainContainer,
			fnGoBack = function(){};

		function renderView(){
			renderer.renderTemplate(view, mainContainer, model);		
		}

		function attachHandlers(){
			$('#back-to-list').on( 'click', function (e) {
				e.preventDefault();		
				//invoke the callback to render previous view			
				fnGoBack();
			});			
		}

		function initialize(pet, viewContainer, callback){
			model = pet;
			//assign container and callback if passed
			mainContainer = viewContainer || mainContainer || $("body");
			fnGoBack = callback || fnGoBack;			
			renderView();
			attachHandlers();
		}		

		return{
			init: function(pet, viewContainer, callback){
				console.log("init pet details presenter");
				initialize(pet, viewContainer, callback);		
			}
		}
	}
);