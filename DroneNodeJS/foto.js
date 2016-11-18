var arDrone = require('ar-drone');
var client = arDrone.createClient();
var fs = require('fs');

var pngStream = client.getPngStream();
var frameCounter = 0;
var period = 5000; // Save a frame every 5000 ms.
var lastFrameTime = 0;

pngStream
  .on('error', console.log)
  .on('data', function(pngBuffer) {
    var now = (new Date()).getTime();
    if (now - lastFrameTime > period) {
      frameCounter++;
      lastFrameTime = now;
      console.log('Saving frame');
      fs.writeFile('frame' + frameCounter + '.png', pngBuffer, function(err) {
        if (err) {
          console.log('Error saving PNG: ' + err);
        }
      });
    }
  });
/*
var fs = require('fs');
fs.writeFile("test", "Hey there!", function(err) {
    if(err) {
        return console.log(err);
    }

    console.log("The file was saved!");
});
*/