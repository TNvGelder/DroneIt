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
		if(clockwiseDegrees > turnto && clockwiseDegrees > (turnto - 180)){
			client.clockwise(0.1);
			console.log('go right');
		}
		else if(clockwiseDegrees < turnto && clockwiseDegrees < (turnto + 180)){
			client.clockwise(-0.1);
			console.log('go left'); 
		} else {
			turn = false;
			client.stop();
			console.log("Turning done");
		}
		console.log(clockwiseDegrees);
	}
});

io.sockets.on('connection', function (socket) {
	console.log(socket.id + " connected");
	
	// Get action
	socket.on('drone', function (action, param) {
		console.log(action + " do " + param + ", received by " + socket.id);
		
		switch(action) {
			// Start
			case "start":			
				client
					.after(10, function() {
						this.takeoff();
					})
					.after(1000, function() {
						this.calibrate(0);
					})
					.after(10000, function() {
						north = clockwiseDegrees;
						console.log('Calibrate done');
						io.sockets.emit('done');
					});		
				
				break;
				
			// Land
			case "land":			
				client.stop();
				client.after(3000, function() {
					this.land();
				});
				
				break;
			
			// Turn
			case "turn":
				north = -60;
				if(north >= 0){
					param = (parseInt(param) + parseInt(north));
				} else if(north < 0){
					param = (parseInt(param) - (parseInt(north) * -1));
				}
				if(param > 180){
					param = (parseInt(param) - 360);
				}
				
				turnto = param;
				turn = true;
				
				break;
			
			// Forward
			case "forward":
				client.front(0.2);
				client.after(param * 1000, function() {
					this.stop();
				});
				
				break;
				
			// Backward
			case "backward":
				client.back(0.2);
				client.after(param * 1000, function() {
					this.stop();
				});
				
				break;
				
			// Left
			case "left":
				client.left(0.2);
				client.after(param * 1000, function() {
					this.stop();
				});
				
				break;
			
			// Right
			case "right":
				client.right(0.2);
				client.after(param * 1000, function() {
					this.stop();
				});
				
				break;
				
			// Rise
			case "rise":			
				client.up(0.5);
				client.after(param * 1000, function() {
					this.stop();
				});		
				
				break;
				
			// Fall
			case "fall":			
				client.down(0.5);
				client.after(param * 1000, function() {
					this.stop();
				});		
				
				break;
		}
		
		if(!turn){
			console.log('Action done');
			io.sockets.emit('done');
		}
	});
});