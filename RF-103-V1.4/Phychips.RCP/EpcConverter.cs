using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Phychips.Rcp
{
    public class EpcConverter
    {
        public const int HEX_STRING = 0;
        public const int ASCII = 1;
        public const int SGTIN96 = 2;    
        public const int EAN13 = 3;    
    
        private const int ID_SGTIN96 = 0x30;
        private static int[] gWeighting = new int[]{ 1, 3, 1, 3, 1, 3, 1, 3, 1, 3, 1, 3 };
        private static int[] gCompanyPrefixBitLen = new int[] {40, 37, 34, 30, 27, 24, 20};

        public static string toTypeString(int type)
        {
	        switch(type)
	        {
	        case HEX_STRING:
	            return "HEX";
	        case ASCII:
	            return "ASCII";
	        case SGTIN96:
	            return "SGTIN96";
	        case EAN13:
	            return "EAN13";
	        }
	
	        return null;	
        }
    
        public static string toAscii(byte[] data)
        {
	        StringBuilder sb = new StringBuilder(data.Length);
	        for (int i = getEpcStartDigit(data); i < data.Length; i++)
	        {	    
	            if (data[i] > 0 && data[i] < 127)
                    sb.Append(System.Text.Encoding.UTF8.GetString(data,i,1));		    
	            else
		            sb.Append(" ");
	        }
	        return sb.ToString();
        }

        static string toHexString(byte[] bytes)
        {
            char[] c = new char[bytes.Length * 2];

            byte b;

            for (int bx = 0, cx = 0; bx < bytes.Length; ++bx, ++cx)
            {
                b = ((byte)(bytes[bx] >> 4));
                c[cx] = (char)(b > 9 ? b - 10 + 'A' : b + '0');

                b = ((byte)(bytes[bx] & 0x0F));
                c[++cx] = (char)(b > 9 ? b - 10 + 'A' : b + '0');
            }

            return new string(c);
        }            
           
        private static string toBinString(byte[] data)
        {
	        if(data.Length < 14)
	            return "";
	
	        int startDigit = getEpcStartDigit(data);
	
	        if(data.Length < 12 + startDigit)
	            return "";
	
	        if(data [startDigit] != ID_SGTIN96) // PC (2 bytes) + EPC
	            return "";

            StringBuilder sb = new StringBuilder();
	
	        for (int i = startDigit + 1; i < data.Length; i++)
	        {

                string bin = Convert.ToString(data[i], 2);
                    
	            if(bin.Length < 8)
	            {
		            for(int j = 0; j < 8 - bin.Length; j++)
		                sb.Append('0');
	            }
	            sb.Append(bin);
	            //sb.append(String.format("%08d", Integer.parseInt(Integer.toBinaryString((data[i]&0xff)))));
	        }   
	
	        //System.out.println("binstring = " + sb.toString());
	
	        return sb.ToString();
        }
    
        private static int getPartition(string bin)
        {	
	        //System.out.println("Partition = " + bin.substring(3, 6) + "[" + Integer.parseInt(bin.substring(3, 6),2) + "]");	
	        return  Convert.ToInt32(bin.Substring(3,3),2);
        }
    
        private static string getCompanyPrefix(string bin) 
        {
	        int nPartition = getPartition(bin);
	        int nCompanyPrefixLen = getCompanyPrefixBitLen(nPartition);
	        string binCompanyPrefix = bin.Substring(6, nCompanyPrefixLen);
	        long nCompanyPrefix = Convert.ToUInt32(binCompanyPrefix, 2);
	        int digitCompanyPrefixLen = 12 - nPartition;
	
	        return padLeft(nCompanyPrefix.ToString(), digitCompanyPrefixLen,"0");
        }
    
        private static int getCompanyPrefixBitLen(int partition)
        {
	        if(partition < 0 || partition > 6)
	            return 0;
	
	        return gCompanyPrefixBitLen[partition];
        }
    
        private static string getItemReference(string bin)
        {
	        int nPartition = getPartition(bin);
	        int nCompanyPrefixLen = getCompanyPrefixBitLen(nPartition);			
	        int nItemReferenceLen = 44 - nCompanyPrefixLen;	
	        string binItemReference = bin.Substring(6 + nCompanyPrefixLen, nItemReferenceLen);
	        long nItemReference = Convert.ToUInt32(binItemReference, 2);
	        int digitItemReferenceLen = 1 + nPartition; 
	
	        return padLeft(nItemReference.ToString(), digitItemReferenceLen,"0");
        }
    
        private static string getSerial(string bin)
        {
	        int nPartition = getPartition(bin);
	        int nCompanyPrefixLen = getCompanyPrefixBitLen(nPartition);			
	        int nItemReferenceLen = 44 - nCompanyPrefixLen;	
	        
            string binSerial 
		        = bin.Substring(6 + nCompanyPrefixLen + nItemReferenceLen, 38);
	        
            long nSerial = Convert.ToUInt32(binSerial, 2);	
	
	        return nSerial.ToString();
        }
    
        private static string getFilter(string bin)
        {
	        return (Convert.ToInt32(bin.Substring(0, 3),2)).ToString();
        }
    
        public static string toSGTIN96(byte[] data)
        {		
	        string bin = toBinString(data);
	    
	        if (bin.Length == 0)
	            return "NON-SGTIN96";
	        else 
	            return getFilter(bin)
		        + "."
		        + getCompanyPrefix(bin)
		        + "."
		        + getItemReference(bin) 
		        + "." 
		        + getSerial(bin);	
        }
    
        public static string toEAN13(byte[] data)
        {		
	        string bin = toBinString(data);
	    
	        if (bin.Length == 0)
	            return "NON-SGTIN96";
	        else 
	        {
	            string message = getCompanyPrefix(bin) + getItemReference(bin).Substring(1);	    
	            return appendParity(message);
	        }
        }
    
        public static string toString(int type, byte[] data)
        {
	        if (type == ASCII)
	        {
	            return toAscii(data);	    
	        }
	        else if(type == SGTIN96)
	        {
	            return toSGTIN96(data);
	        }
	        else if(type == EAN13)
	        {
	            return toEAN13(data);
	        }
	        else
	        {
	            return toHexString(data);
	        }
        }
    
        private static int getEpcStartDigit(byte[] data)
        {
	        int digit = 2; // PC
	
	        if((data[0] & 0x02) == 0x02)
	        {   
	            if((data[2] & 0x08) == 0x08)
	            {
		        digit = 6; // PC + XPC_W1 + XPC_W2
	            }
	            else
	            {
		        digit = 4; // PC + XPC_W1
	            }
	        }	
	
	        return digit;
        }
    
        // 	need to optimize
        private static string padLeft(string s, int n, string pad) 
        {
            //	System.out.println("s = " + s);
            //	System.out.println("n = " + n);
            //	System.out.println("padLeft = " + String.format("%1$" + n + "s", s).replace(" ", pad));
	        StringBuilder sb = new StringBuilder();
	        if(s.Length < n)
	        {
	            for(int i = 0; i < n - s.Length; i++)
	            {
		            sb.Append(pad);
	            }
	    
	        }
	        sb.Append(s);
            //	System.out.println("sb = " + sb.toString());
	        
            return sb.ToString();
	        //return String.format("%1$" + n + "s", s).replace(" ", pad);  
        }
    
        private static string appendParity(string message)  
        {  	
            int sum = 0;  
            int parity = 0;  

            for(int i = 0; i < 12; i++)  
            {  
                sum += int.Parse(message.Substring(i, 1)) * gWeighting[i];  
            }  

            parity = 10 - (sum % 10);  
            if (parity == 10)  
            {  
                parity = 0;  
            }

            return message + parity.ToString();
        }  
    }
}
