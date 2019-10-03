/*
using java.lang.reflect.Method;

using android.bluetooth.BluetoothDevice;
using System;

public class Utils {

    public static bool autoBond(Class btClass,BluetoothDevice device,String strPin) throws Exception {
    	Method autoBondMethod = btClass.getMethod("setPin",new Class[]{byte[].class});
    	bool result = (Boolean)autoBondMethod.invoke(device,new Object[]{strPin.getBytes()}); 
    	return result;
    }
    public static bool createBond(Class btClass,BluetoothDevice device) throws Exception {
    	Method createBondMethod = btClass.getMethod("createBond"); 
    	bool returnValue = (Boolean) createBondMethod.invoke(device);
    	return returnValue.booleanValue();
    }
	public static  int getRawWaveValue(byte highOrderByte, byte lowOrderByte)
	 {
		   int hi = ((int)highOrderByte)& 0xFF;
		   int lo = ((int)lowOrderByte) & 0xFF;
		   return( (hi<<8) | lo );
	 }
    
	public static String byte2String( byte[] b) {  
		StringBuffer sb = new StringBuffer();
		   for (int i = 0; i < b.length; i++) { 
		     String hex = IntegeResources.toHexString(b[i] & 0xFF); 
		     if (hex.length() == 1) { 
		       hex = '0' + hex; 
		     } 
		     sb.append(hex);
		   } 
		   return sb.toString().toLowerCase();
		}
}
*/