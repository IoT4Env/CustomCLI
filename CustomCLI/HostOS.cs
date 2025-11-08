using System.Runtime.InteropServices;

namespace CustomCLI;


public sealed class HostOS
{
	public static char PathDelimiter { get; private set; }
	private static HostOS? _instance;
	private static readonly object _locker = new();

	/// <summary>
	/// Private constructor.
	/// Configures this class variables based on the OS this project is fired from.
	/// </summary>
	private HostOS()
	{
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			PathDelimiter = '/';
		else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			PathDelimiter = '\\';
	}

	/// <summary>
	/// Instantiate an object of this class.
	/// No more than one instance can exist simultaniusly
	/// </summary>
	/// <returns>This class instances if it was not instantiated yet</returns>
	public static HostOS Instantiate()
	{
		lock (_locker)
		{
			return _instance == null ? new HostOS() : _instance;
		}
	}
}