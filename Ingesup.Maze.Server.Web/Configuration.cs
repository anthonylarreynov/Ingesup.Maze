using Ingesup.Maze.Server.Web.Models;
using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Drawing;
using System.Web;

namespace Ingesup.Maze.Server.Web
{
    public class Configuration
    {
        private static volatile Configuration _instance;
        private static object _syncRoot = new Object();

        private string _appDataDirectory = null;
        private ConcurrentDictionary<Difficulty, Size> _difficultySizes = null;
        private int? _movePlayerMinInterval = null;
        private int? _mazeRefreshInterval = null;
        private string _lastGameKey = null;

        private Configuration()
        {
        }

        public static Configuration Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new Configuration();
                        }
                    }
                }

                return _instance;
            }
        }

        public string AppDataDirectory
        {
            get
            {
                if (_appDataDirectory == null)
                {
                    _appDataDirectory = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["AppDataDirectory"]);
                }

                return _appDataDirectory;
            }
        }

        public ConcurrentDictionary<Difficulty, Size> DifficultySizes
        {
            get
            {
                if (_difficultySizes == null)
                {
                    _difficultySizes = new ConcurrentDictionary<Difficulty, Size>();
                    foreach (Difficulty difficulty in Enum.GetValues(typeof(Difficulty)))
                    {
                        _difficultySizes.TryAdd(difficulty, ParseDifficultySize(ConfigurationManager.AppSettings[string.Format("Difficulty{0}Size", difficulty)]));
                    }
                }

                return _difficultySizes;
            }
        }

        public int MaxPlayersCount
        {
            get { return 4; }
        }

        public int MovePlayerMinInterval
        {
            get
            {
                if (!_movePlayerMinInterval.HasValue)
                {
                    _movePlayerMinInterval = int.Parse(ConfigurationManager.AppSettings["MovePlayerMinInterval"]);
                }

                return _movePlayerMinInterval.Value;
            }
        }

        public int MazeRefreshInterval
        {
            get
            {
                if (!_mazeRefreshInterval.HasValue)
                {
                    _mazeRefreshInterval = int.Parse(ConfigurationManager.AppSettings["MazeRefreshInterval"]);
                }

                return _mazeRefreshInterval.Value;
            }
        }

        public string LastGameKey
        {
            get
            {
                if (_lastGameKey == null)
                {
                    _lastGameKey = ConfigurationManager.AppSettings["LastGameKey"];
                }

                return _lastGameKey;
            }
        }

        private static Size ParseDifficultySize(string value)
        {
            string[] values = value.Split('x');
            return new Size(int.Parse(values[0]), int.Parse(values[1]));
        }
    }
}