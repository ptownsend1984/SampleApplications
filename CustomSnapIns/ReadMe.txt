To deploy the Snap-In:

Make sure that you have RunInstaller(true) on the PSSnapIn derived class.
Copy the built DLL to somewhere safe (C:\SnapIns).
Run both:
C:\Windows\Microsoft.NET\Framework\v2.0.50727\installutil.exe "C:\SnapIns\MyDll.dll"
C:\Windows\Microsoft.NET\Framework64\v2.0.50727\installutil.exe "C:\SnapIns\MyDll.dll"

Restart any PowerShell instances.
Run this to verify that it has installed properly:
Get-PSSnapin -Registered

To add the SnapIn:
Add-SnapIn MySnapIn