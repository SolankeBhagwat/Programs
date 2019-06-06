using System;

public sealed class Logger
{
    private static Logger _logger = null;
    private Logger()
    {
    }
    private static object obj = null;
    public static Logger GetLogger()
    {
        lock (obj)
        {
            if (_logger == null)
            {
                _logger = new Logger();
            }
        }

        return _logger;
    }

}