using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WMPLib;

namespace DroneControl {
    public class Sound {
        private static volatile Sound _instance;
        private static object syncRoot = new Object();
        public static Sound Instance {
            get {
                if (_instance == null) {
                    lock (syncRoot) {
                        if (_instance == null)
                            _instance = new Sound();
                    }
                }
                return _instance;
            }
        }
        private string _path;
        private WindowsMediaPlayer _player;

        private Sound() {
            _path = "Sounds/";
            _player = new WindowsMediaPlayer();
            if (!Directory.Exists(_path))
                Directory.CreateDirectory(_path);
        }

        private void play(string fileName) {
            if (!File.Exists(_path + fileName))
                File.Copy("../../" + _path + fileName, _path + fileName);

            _player.URL = _path + fileName;
            _player.controls.play();
        }

        public void Lazer() {
            play("LAZER.WAV");
        }

        public void Yoda() {
            play("YODA3.WAV");
        }

        public void R2D2a() {
            play("R2D2a.WAV");
        }

        public void R2D2b() {
            play("R2D2b.WAV");
        }

        public void R2D2c() {
            play("R2D2c.WAV");
        }

        public void R2D2d() {
            play("R2D2d.WAV");
        }

        public void R2D2e() {
            play("R2D2e.WAV");
        }

        public void R2D2f() {
            play("R2D2f.WAV");
        }

        public void Harmen() {
            play("Harmen.WAV");
        }
    }
}
