var app = require('http').createServer();
var io = require('socket.io').listen(app);
var arDrone = require('ar-drone');
var client  = arDrone.createClient();

app.listen(8000);
console.log("listening on port 8000");

io.sockets.on('connection', function (socket) {
	console.log(socket.id + " connected");
	
	// Take off
	client
	  .after(10, function() {
		this.takeoff();
	  })
	  .after(1000, function() {
		this.calibrate(0);
	  });
	
	// Get action
	socket.on('drone', function (action, param) {
		console.log(action + " do " + param + ", received by " + socket.id);
		
		switch(action) {
			// Turn
			case "turn":
				if(param == 45)
					client.clockwise(0.175);
				else if (param == 90)
					client.clockwise(0.35);
				else if (param == 180)
					client.clockwise(0.7);
				else if(param == -45)
					client.clockwise(-0.175);
				else if (param == -90)
					client.clockwise(-0.35);
				else if (param == -180)
					client.clockwise(-0.7);
				
				break;
			
			// Forward
			case "forward":
				client.front(0.5);
				client.after(param * 1000, function() {
					this.stop();
				});
				
				break;
				
			// Backwards
			case "backwards":
				client.back(0.5);
				client.after(param * 1000, function() {
					this.stop();
				});
				
				break;
		}
	});
	
	// Send navdata
	/*client.on('navdata', function (){
		io.sockets.emit('navdata', data);
	});*/
});