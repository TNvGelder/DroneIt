var arDrone = require('ar-drone');
var client  = arDrone.createClient();

client
  .after(5000, function() {
    this.takeoff();
  })
  .after(3000, function() {
    this.stop();
    this.land();
  });