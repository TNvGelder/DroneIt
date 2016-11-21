var arDrone = require('ar-drone');
var client = arDrone.createClient();
var clockwiseDegrees = 0;


client.on('navdata', function(navdata) {
	if(navdata.demo != null){
		clockwiseDegrees = parseInt(navdata.demo.clockwiseDegrees);
	}
});

client
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
  
  