var arDrone = require('ar-drone');
var client  = arDrone.createClient();

client
.after(100, function() {
    this.takeoff();
  })
.after(10000, function() {
    this.stop();
  })
  .after(3000, function() {
    this.land();
  });