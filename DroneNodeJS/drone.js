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

function ActionDone(){
	if(!turn){
		setTimeout(function () {
			console.log('Action done');
			io.sockets.emit('done');
		}, 5000); 
	}
}

client.on('navdata', function(navdata) {
	if(navdata.demo != null){
		clockwiseDegrees = parseInt(navdata.demo.clockwiseDegrees);
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
			ActionDone();
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
					.after(1000, function() {
						this.calibrate(0);
						console.log('Calibrating...');
					})
					.after(5000, function() {
						this.takeoff();
						console.log('Taking off');
					})
					.after(20000, function() {
						north = clockwiseDegrees;
						console.log('Calibrate done');
						ActionDone();
					});
				
				break;
				
			// Land
			case "land":			
				client.stop();
				client.after(3000, function() {
					this.land();
					ActionDone();
				});
				
				break;
			
			// Turn
			case "turn":
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
				turnto = clockwiseDegrees;
				client.front(0.2);
				client.after(param * 625, function() {
					this.stop();
					turn = true;
				});
				
				break;
				
			// Backward
			case "backward":
				turnto = clockwiseDegrees;
				client.back(0.2);
				client.after(param * 625, function() {
					this.stop();
					turn = true;
				});
				
				break;
				
			// Left
			case "left":
				turnto = clockwiseDegrees;
				client.left(0.2);
				client.after(param * 625, function() {
					this.stop();
					turn = true;
				});
				
				break;
			
			// Right
			case "right":
				turnto = clockwiseDegrees;
				client.right(0.2);
				client.after(param * 625, function() {
					this.stop();
					turn = true;
				});
				
				break;
				
			// Rise
			case "rise":
				turnto = clockwiseDegrees;
				client.up(0.5);
				client.after(param * 1000, function() {
					this.stop();
					turn = true;
				});
				
				break;
				
			// Fall
			case "fall":	
				turnto = clockwiseDegrees;
				client.down(0.5);
				client.after(param * 1000, function() {
					this.stop();
					turn = true;
				});		
				
				break;
		}
	});
});