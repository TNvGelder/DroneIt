/*var arDrone = require('ar-drone');
var client  = arDrone.createClient();

client.config('general:navdata_demo', true);

function test(data){
	var demo = data.demo;
	console.log(demo.controlState);
}

client.on('navdata', test);*/

var arDrone = require('ar-drone');
var droneClient = arDrone.createClient();
//droneClient.config('general:navdata_demo', true);
var clockwiseDegrees = 0;


droneClient.on('navdata', function(navdata) {
	if(navdata.demo != null){
		clockwiseDegrees = parseInt(navdata.demo.clockwiseDegrees);
		
	}
});

droneClient
  .after(10, function() {
    this.takeoff();
  })
  .after(1000, function() {
	this.calibrate(0);
  })
  .after(10000, function() {
		while(clockwiseDegrees < 90){
			this.clockwise(0.5);
			console.log(clockwiseDegrees);
		}
		
		while(clockwiseDegrees > 90){
			this.clockwise(-0.5);
			console.log(clockwiseDegrees);
		}
		
		console.log("done");
  })
  /*.after(10000, function() {
    this.front(0.1);
  })*/
  /*.after(5000, function() {
    this.stop();
  })*/
  /*.after(4000, function() {
    this.stop();
  })
  .after(4000, function() {
    this.front(0.1);
  })*/
  .after(10000, function() {
    this.stop();
  })
  .after(3000, function() {
    this.land();
  });
  
  