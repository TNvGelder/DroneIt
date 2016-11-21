var arDrone = require('ar-drone');
var client  = arDrone.createClient();

client.config('general:navdata_demo', true);

function test(data){
	var demo = data.demo;
	console.log(demo.controlState);
}

client.on('navdata', test);

/*client
  .after(10, function() {
    this.takeoff();
  })
  .after(1000, function() {
	this.calibrate(0);
  })*/
  /*.after(10000, function() {
    this.clockwise(0.35);
  })*/
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
  /*.after(4000, function() {
    this.stop();
  })
  .after(3000, function() {
    this.land();
  });*/
  
  