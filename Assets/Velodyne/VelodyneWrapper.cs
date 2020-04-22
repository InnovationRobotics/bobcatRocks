
using System;
using System.Runtime.InteropServices;

public class VelodyneWrapper : IDisposable {
	const String DLL_LOCATION = "libvelodyne";

	[DllImport (DLL_LOCATION)]
	private static extern IntPtr Velodyne16CreateObject(string confFilePath);


    [DllImport(DLL_LOCATION)]
    private static extern IntPtr Velodyne16CreateObjectNew(string IpAddress, string Port, string ReturnMode, string DataSource);

    [DllImport (DLL_LOCATION)]
	private static extern IntPtr Velodyne32CreateObject(string confFilePath);

    [DllImport(DLL_LOCATION)]
    private static extern IntPtr Velodyne32CreateObjectNew(string IpAddress, string Port, string ReturnMode, string DataSource);

    [DllImport (DLL_LOCATION)]
	private static extern void VelodyneDeleteObject(IntPtr pObj);

	[DllImport (DLL_LOCATION)]
	private static extern void VelodyneSetAzimuth(IntPtr pObj, double azimuth);

	[DllImport (DLL_LOCATION)]
	private static extern void VelodyneSetTimeStamp(IntPtr pObj, double timeStamp);

	[DllImport (DLL_LOCATION)]
	private static extern void VelodyneSetChannel(IntPtr pObj, double distance, short reflectivity);

	[DllImport (DLL_LOCATION)]
	private static extern void VelodyneCloseBlock(IntPtr pObj);

	[DllImport (DLL_LOCATION)]
	private static extern void VelodyneSendData(IntPtr pObj);

    [DllImport(DLL_LOCATION)]
    private static extern void TestVelodyneOrientation(IntPtr pObj, float azimuth, float pitch, float roll);

    private IntPtr m_nativeObject;

	public VelodyneWrapper(string confFilePath, bool isVelodyne16) {
		if (isVelodyne16) {
			this.m_nativeObject = Velodyne16CreateObject(confFilePath);
		}
		else {
			this.m_nativeObject = Velodyne32CreateObject(confFilePath);
		}
			
	}

    public VelodyneWrapper(string IpAddress, string Port, string ReturnMode, string DataSource, bool isVelodyne16) {
        if (isVelodyne16)
        {
            this.m_nativeObject = Velodyne16CreateObjectNew(IpAddress, Port, ReturnMode, DataSource);
        }
        else
        {
            this.m_nativeObject = Velodyne32CreateObjectNew(IpAddress, Port, ReturnMode, DataSource);
        }

    }

    ~VelodyneWrapper() {Dispose(false);}
	
	public void Dispose() { Dispose(true);}

    protected virtual void Dispose(bool bDisposing) {
        if (this.m_nativeObject != IntPtr.Zero) {
            VelodyneDeleteObject(this.m_nativeObject);
            this.m_nativeObject = IntPtr.Zero;
        }

        if (bDisposing) {
            GC.SuppressFinalize(this);
        }
    }

	public void SetAzimuth(double azimuth) {
		VelodyneSetAzimuth(this.m_nativeObject, azimuth);
	}

	public void SetTimeStamp(double timeStamp) {
		VelodyneSetTimeStamp(this.m_nativeObject, timeStamp);
	}

	public void SetChannel(double distance, short reflectivity) {
		VelodyneSetChannel(this.m_nativeObject, distance, reflectivity);
	}

	public void CloseBlock() {
		VelodyneCloseBlock(this.m_nativeObject);
	}

	public void SendData() {
		VelodyneSendData(this.m_nativeObject);
	}

    public void TestOrientation(float azimuth, float pitch, float roll)
    {
        TestVelodyneOrientation(this.m_nativeObject, azimuth, pitch, roll);
    }
}