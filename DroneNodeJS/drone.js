var app = require('http').createServer();
var io = require('socket.io').listen(app);
var arDrone = require('ar-drone');
var client  = arDrone.createClient();
var clockwiseDegrees = null;
var north = 0;
var turn = false;
var turnto = 0;


app.listen(8000);
console.log("listening on port 8000");

client.on('navdata', function(navdata) {
	if(navdata.demo != null){
		clockwiseDegrees = parseInt(navdata.demo.clockwiseDegrees);
		console.log(navdata);
	}
	
	if(turn){							
		if(clockwiseDegrees < turnto || clockwiseDegrees > (turnto - 180)){
			client.clockwise(0.2);
			console.log('go right');
		}
		if(clockwiseDegrees > turnto || clockwiseDegrees > (turnto + 180)){
			client.clockwise(-0.2);
			console.log('go left'); 
		}
		console.log(clockwiseDegrees);
	}
});

io.sockets.on('connection', function (socket) {
	console.log(socket.id + " connected");
	
	// Take off
	client
	  .after(10, function() {
		this.takeoff();
	  })
	  .after(1000, function() {
		this.calibrate(0);
	  })
	  .after(10000, function() {
		north = clockwiseDegrees;  
		io.sockets.emit('done');
	  });
	  
	
	
	// Get action
	socket.on('drone', function (action, param) {
		console.log(action + " do " + param + ", received by " + socket.id);
		
		switch(action) {
			// Turn
			case "turn":			
				turn = true;
				turnto = param;		
				
				break;
			
			// Forward
			case "forward":
				client.front(0.5);
				client.after(param * 1000, function() {
					client.stop();
				});
				
				break;
				
			// Backward
			case "backward":
				client.back(0.5);
				client.after(param * 1000, function() {
					client.stop();
				});
				
				break;
		}
		
		if(!turn){
			setTimeout(console.log('Action done'), 3000);
			io.sockets.emit('done');
		}
	});
});