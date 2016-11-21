var arDrone = require('ar-drone');
var client  = arDrone.createClient();

client.on('navdata', console.log);

/*client
  .after(10, function() {
    this.takeoff();
  })
  .after(1000, function() {
	this.calibrate(0);
  })
  .after(10000, function() {
    this.clockwise(0.7);
  })*/
  /*.after(10000, function() {
    this.front(0.5);
  })*/
  /*.after(5000, function() {
    this.stop();
  })
  .after(5000, function() {
    this.land();
  })
    .after(5000, function() {
    this.stop();
  });*/
  
  