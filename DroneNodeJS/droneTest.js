var arDrone = require('ar-drone');
var client  = arDrone.createClient();
var clockwiseDegrees = null;
var north = 0;
var turn = false;
var turnto = 0;

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
			console.log("Turning done");
		}
		console.log(clockwiseDegrees);
	}
});

client
  .after(10, function() {
    this.takeoff();
  })
  /*.after(1000, function() {
	this.calibrate(0);
  })*/
  /*.after(10000, function() {
	north = clockwiseDegrees;
	console.log('Calibrate done');
	this.stop();
  })*/
  /*.after(3000, function() {
	  turnto = 90;
	  turn = true;
  })*/
  /*.after(2000, function() {
	this.up(0.5);
  })
  .after(5 * 1000, function() {
	this.stop();
  })
  .after(2000, function() {
	this.front(0.2);
  })
  .after(5 * 1000, function() {
	this.stop();
  })
  .after(2000, function() {
	this.left(0.2);
  })
  .after(5 * 1000, function() {
	this.stop();
  })
  .after(2000, function() {
	this.back(0.2);
  })
  .after(5 * 1000, function() {
	this.stop();
  })
  .after(2000, function() {
	this.right(0.2);
  })
  .after(5 * 1000, function() {
	this.stop();
  })
  .after(2000, function() {
	this.down(0.5);
  })
  .after(5 * 1000, function() {
	this.stop();
  })*/
  .after(3000, function() {
    this.stop();
  })
  .after(10000, function() {
    this.land();
  });
  
  