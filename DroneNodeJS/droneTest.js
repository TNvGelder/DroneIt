/**
 * @author: Gerhard Kroes
**/
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
	this.calibrate(0);
  })
  .after(5000, function() {
    this.takeoff();
  })
  .after(20000, function() {
	north = clockwiseDegrees;
	console.log('Calibrate done');
  })
  /*.after(5000, function() {
	this.front(0.2);
  })
  .after(5 * 1000, function() {
	this.stop();
  })
  .after(5000, function() {
	this.back(0.2);
  })
  .after(5 * 1000, function() {
	this.stop();
  })*/
  /*.after(5000, function() {
	  var param = 90;
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
  })*/
  /*.after(5000, function() {
	this.up(0.5);
  })
  .after(5 * 1000, function() {
	this.stop();
  })*/
  /*.after(5000, function() {
	this.front(0.2);
  })
  .after(2 * 1000, function() {
	this.stop();
  })
  .after(5000, function() {
	this.left(0.2);
  })
  .after(2 * 1000, function() {
	this.stop();
  })
  .after(5000, function() {
	this.back(0.2);
  })
  .after(2 * 1000, function() {
	this.stop();
  })
  .after(5000, function() {
	this.right(0.2);
  })
  .after(2 * 1000, function() {
	this.stop();
  })*/
  /*.after(5000, function() {
	this.down(0.5);
  })
  .after(5 * 1000, function() {
	this.stop();
  })*/
  .after(20000, function() {
    this.stop();
  })
  .after(3000, function() {
    this.land();
  });