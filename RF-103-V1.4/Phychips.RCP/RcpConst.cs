//! Copyright (C) 2007 Phychips
//! 
//! RcpConst.cs
//!
//! Description
//! 	Rcp Constant
//!-------------------------------------------------------------------
//! History
//!-------------------------------------------------------------------
//! 1.0	2007/09/01	Jinsung Yi		Initial Release

using System;
using System.Collections.Generic;
using System.Text;

namespace Phychips.Rcp
{
    public class RcpConst
    {
        // RCP Command Type
        public const byte RCP_CMD_NONE = 0x00;
        public const byte RCP_CMD_CTL_RD_PWR = 0x01;        // Reader Power Control
        public const byte RCP_CMD_NIMPLMNT_00 = 0x02;
        public const byte RCP_CMD_GET_RD_INF = 0x03;        // Get Reader Information
        public const byte RCP_CMD_NIMPLMNT_01 = 0x04;
        public const byte RCP_CMD_NIMPLMNT_02 = 0x05;
        public const byte RCP_CMD_GET_REGION = 0x06;        // Get Region
        public const byte RCP_CMD_SET_REGION = 0x07;        // Set Resion
        public const byte RCP_CMD_CTL_RESET = 0x08;         // Reset Reader
        public const byte RCP_CMD_NIMPLMNT_03 = 0x09;       // Get Type B A/I Parameters
        public const byte RCP_CMD_NIMPLMNT_04 = 0x0A;       // Set Type B A/I Parameters
        public const byte RCP_CMD_GET_C_SEL_PARM = 0x0B;    // Get Type C A/I Select Parameters
        public const byte RCP_CMD_SET_C_SEL_PARM = 0x0C;    // Set Type C A/I Select Parameters
        public const byte RCP_CMD_GET_C_QRY_PARM = 0x0D;    // Get Type C A/I Query Related Parameters
        public const byte RCP_CMD_SET_C_QRY_PARM = 0x0E;    // Set Type C A/I Query Related Parameters
        public const byte RCP_CMD_GET_AT_RD_PARM = 0x0F;    // Get Automatic Read Parameters

        public const byte RCP_CMD_SET_AT_RD_PARM = 0x10;    // Set Automatic Read Parameters
        public const byte RCP_CMD_GET_CH = 0x11;            // Get Frequency Channel
        public const byte RCP_CMD_SET_CH = 0x12;            // Set Frequency Channel
        public const byte RCP_CMD_GET_FH_LBT = 0x13;        // Get Frequency Hopping
        public const byte RCP_CMD_SET_FH_LBT = 0x14;        // Set Frequency Hopping
        public const byte RCP_CMD_GET_TX_PWR = 0x15;
        public const byte RCP_CMD_SET_TX_PWR = 0x16;
        public const byte RCP_CMD_SET_CW = 0x17;
        public const byte RCP_CMD_NIMPLMNT_07 = 0x18;       // #define RCP_CMD_SET_SECURITY_KEY	0x18
        public const byte RCP_CMD_NIMPLMNT_08 = 0x19;       // #define RCP_CMD_SET_FREQ_MODE  		0x19
        public const byte RCP_CMD_NIMPLMNT_09 = 0x1A;       // #define RCP_CMD_SET_MAC_CTRL  		0x1A
        public const byte RCP_CMD_SET_ANT = 0x1B;           // #define RCP_CMD_NIMPLMNT_10			0x1B 
        public const byte RCP_ISO_RESERVED_00 = 0x1C;
        public const byte RCP_ISO_RESERVED_01 = 0x1D;       // #define RCP_CMD_SET_SINGLETONE		0x1D
        public const byte RCP_ISO_RESERVED_02 = 0x1E;
        public const byte RCP_ISO_RESERVED_03 = 0x1F;

        public const byte RCP_UNDEF_00 = 0x20;              // #define RCP_CMD_CLR_SINGLETONE		0x20
        public const byte RCP_CMD_NIMPLMNT_11 = 0x21;       // #define RCP_CMD_READ_B_UII			0x21 // Read Type B UII
        public const byte RCP_CMD_READ_C_UII = 0x22;        // Read Type C UII
        public const byte RCP_CMD_READ_C_UII_RSSI = 0x23;
        public const byte RCP_CMD_READ_C_USER_DT = 0x24;    // Read Type C User Data
        public const byte RCP_CMD_READ_C_UII_TID = 0x25;
        public const byte RCP_CMD_READ_C_EX2 = 0x26;
        public const byte RCP_CMD_STRT_AUTO_READ = 0x27;    // Start Automatic Read
        public const byte RCP_CMD_STOP_AUTO_READ = 0x28;    // Stop Automatic Read
        public const byte RCP_CMD_READ_C_DT = 0x29;         // Read Type C Data
        public const byte RCP_CMD_READ_C_DT_EX = 0x2A;      // Read Type C Data Long
        public const byte RCP_VENDOR_SPECIFIC_09 = 0x2B;
        public const byte RCP_VENDOR_SPECIFIC_10 = 0x2C;
        public const byte RCP_VENDOR_SPECIFIC_11 = 0x2D;
        public const byte RCP_CMD_GET_SESSION = 0x2E;       // #define RCP_VENDOR_SPECIFIC_12  	0x2E
        public const byte RCP_CMD_SET_SESSION = 0x2F;       // #define RCP_VENDOR_SPECIFIC_13  	0x2F

        public const byte RCP_CMD_GET_HOPPING_TBL = 0x30; 
        public const byte RCP_CMD_SET_HOPPING_TBL = 0x31;
        public const byte RCP_CMD_GET_MODULATION = 0x32;
        public const byte RCP_CMD_SET_MODULATION = 0x33;
        public const byte RCP_CMD_GET_ANTICOL_MODE = 0x34;
        public const byte RCP_CMD_SET_ANTICOL_MODE = 0x35;
        public const byte RCP_CMD_STRT_AUTO_READ_EX = 0x36; // Start Automatic Read
        public const byte RCP_CMD_STOP_AUTO_READ_EX = 0x37; // Stop Automatic Read
        public const byte RCP_CMD_STRT_AUTO_READ_RSSI = 0x38;
        public const byte RCP_CMD_STOP_AUTO_READ_RSSI = 0x39;
        public const byte RCP_CMD_STRT_AUTO_READ_EX2 = 0x3A;
        public const byte RCP_CMD_NIMPLMNT_28 = 0x3B;
        public const byte RCP_CMD_NIMPLMNT_29 = 0x3C;
        public const byte RCP_ISO_RESERVED_04 = 0x3D;
        public const byte RCP_ISO_RESERVED_05 = 0x3E;
        public const byte RCP_ISO_RESERVED_06 = 0x3F;

        public const byte RCP_CMD_NIMPLMNT_30 = 0x40;
        public const byte RCP_CMD_NIMPLMNT_31 = 0x41;
        public const byte RCP_CMD_NIMPLMNT_32 = 0x42;
        public const byte RCP_CMD_NIMPLMNT_33 = 0x43;
        public const byte RCP_CMD_NIMPLMNT_34 = 0x44;
        public const byte RCP_CMD_NIMPLMNT_35 = 0x45;
        public const byte RCP_CMD_WRITE_C_DT = 0x46;        // Write Type C User Data
        public const byte RCP_CMD_BLOCKWRITE_C_DT = 0x47;   // BlockWrite Type C User Data
        public const byte RCP_CMD_BLOCKERASE_C_DT = 0x48;   // BlockErase Type C User Data


// >> 20200106, HYO        
        //public const byte RCP_VENDOR_SPECIFIC_25 = 0x49;
        //public const byte RCP_VENDOR_SPECIFIC_26 = 0x4A;
        public const byte RCP_CMD_WRITE_C_DT_MASK_TID = 0x49;
        public const byte RCP_CMD_BLOCKWRITE_C_DT_MASK_TID = 0x4A;
// << 20200106, HYO
        
        
        public const byte RCP_VENDOR_SPECIFIC_27 = 0x4B;
        public const byte RCP_VENDOR_SPECIFIC_28 = 0x4C;
        public const byte RCP_CMD_TRANSPORT = 0x4D;
        public const byte RCP_VENDOR_SPECIFIC_30 = 0x4E;
        public const byte RCP_VENDOR_SPECIFIC_31 = 0x4F;

        public const byte RCP_VENDOR_SPECIFIC_32 = 0x50;
        public const byte RCP_VENDOR_SPECIFIC_33 = 0x51;
        public const byte RCP_CMD_NXP_READPROTECT = 0x52;
        public const byte RCP_CMD_NXP_RESET_READPROTECT = 0x53;
        public const byte RCP_CMD_NXP_CHANGE_EAS = 0x54;
        public const byte RCP_CMD_NXP_EAS_ALARM = 0x55;
        public const byte RCP_CMD_NXP_CALIBRATE = 0x56;
        public const byte RCP_CMD_NXP_CHANGESTATUSWORD = 0x57;
        public const byte RCP_CMD_NXP_READ_SIGNATURE = 0x58;
        public const byte RCP_ISO_RESERVED_08 = 0x59;
        public const byte RCP_ISO_RESERVED_09 = 0x5A;
        public const byte RCP_ISO_RESERVED_10 = 0x5B;
        public const byte RCP_ISO_RESERVED_11 = 0x5C;
        public const byte RCP_ISO_RESERVED_12 = 0x5D;
        public const byte RCP_ISO_RESERVED_13 = 0x5E;
        public const byte RCP_ISO_RESERVED_14 = 0x5F;

        public const byte RCP_UNDEF_01 = 0x60;
        public const byte RCP_CMD_NIMPLMNT_36 = 0x61;
        public const byte RCP_VENDOR_SPECIFIC_40 = 0x62;
        public const byte RCP_VENDOR_SPECIFIC_41 = 0x63;
        public const byte RCP_VENDOR_SPECIFIC_42 = 0x64;
        public const byte RCP_CMD_KILL_RECOM_C = 0x65;      // Kill Type C Tag

        
// >> 20180611, HYO        
        //public const byte RCP_VENDOR_SPECIFIC_44 = 0x66;
        public const byte RCP_CMD_SET_GAIN_MODE = 0x66;
// << 20180611, HYO        
        
        
        public const byte RCP_VENDOR_SPECIFIC_45 = 0x67;
        public const byte RCP_VENDOR_SPECIFIC_46 = 0x68;
        public const byte RCP_VENDOR_SPECIFIC_47 = 0x69;
        public const byte RCP_VENDOR_SPECIFIC_48 = 0x6A;
        public const byte RCP_VENDOR_SPECIFIC_49 = 0x6B;
        public const byte RCP_PHY_RESERVED_03 = 0x6C;       // RCP_VENDOR_SPECIFIC_50
        public const byte RCP_CMD_CUSTOM_01 = 0x6D;         // RCP_VENDOR_SPECIFIC_51
        public const byte RCP_CMD_CUSTOM_02 = 0x6E;         // RCP_VENDOR_SPECIFIC_52
        public const byte RCP_CMD_CUSTOM_03 = 0x6F;         // RCP_VENDOR_SPECIFIC_53

        public const byte RCP_CMD_CUSTOM_04 = 0x70;         // RCP_VENDOR_SPECIFIC_54
        public const byte RCP_CMD_CUSTOM_05 = 0x71;         // RCP_VENDOR_SPECIFIC_55
        public const byte RCP_CMD_CUSTOM_06 = 0x72;         // RCP_VENDOR_SPECIFIC_56
        public const byte RCP_CMD_CUSTOM_07 = 0x73;         // RCP_VENDOR_SPECIFIC_57
        public const byte RCP_CMD_CUSTOM_08 = 0x74;         // RCP_VENDOR_SPECIFIC_58
        public const byte RCP_CMD_CUSTOM_09 = 0x75;         // RCP_VENDOR_SPECIFIC_59		
        public const byte RCP_CMD_CUSTOM_10 = 0x76;         // RCP_VENDOR_SPECIFIC_60		
        public const byte RCP_CMD_CUSTOM_11 = 0x77;         // RCP_VENDOR_SPECIFIC_61		
        public const byte RCP_ISO_RESERVED_15 = 0x78;
        public const byte RCP_ISO_RESERVED_16 = 0x79;
        public const byte RCP_ISO_RESERVED_17 = 0x7A;
        public const byte RCP_ISO_RESERVED_18 = 0x7B;
        public const byte RCP_ISO_RESERVED_19 = 0x7C;
        public const byte RCP_ISO_RESERVED_20 = 0x7D;
        public const byte RCP_ISO_RESERVED_21 = 0x7E;
        public const byte RCP_ISO_RESERVED_22 = 0x7F;

        public const byte RCP_UNDEF_02 = 0x80;
        public const byte RCP_CMD_NIMPLMNT_37 = 0x81;       // Lock Type B Tag
        public const byte RCP_CMD_LOCK_C = 0x82;            // Lock Type C Tag
        public const byte RCP_CMD_BLOCKPERMALOCK_C = 0x83;  // Block Permalock
        public const byte RCP_CMD_AUTHENTICATE = 0x84;      // Authenticate Tag
        public const byte RCP_CMD_UNTRACEABLE = 0x85;       // Untraceable tag
        public const byte RCP_CMD_AUTHENTICATE_EX = 0x86;	// #define RCP_VENDOR_SPECIFIC_65		0x86
        public const byte RCP_CMD_UNTRACEABLE_EX = 0x87;	// #define RCP_VENDOR_SPECIFIC_66		0x87
        public const byte RCP_VENDOR_SPECIFIC_67 = 0x88;
        public const byte RCP_VENDOR_SPECIFIC_68 = 0x89;


// >> 20200106, HYO        
        //public const byte RCP_VENDOR_SPECIFIC_69 = 0x8A;
        public const byte RCP_CMD_LOCK_C_MASK_TID = 0x8A;
// << 20200106, HYO        

        
        public const byte RCP_VENDOR_SPECIFIC_70 = 0x8B;
        public const byte RCP_VENDOR_SPECIFIC_71 = 0x8C;
        public const byte RCP_CMD_SET_MODEM92_REG = 0x8D;
        public const byte RCP_CMD_GET_MODEM92_REG = 0x8E;
        public const byte RCP_VENDOR_SPECIFIC_74 = 0x8F;

        public const byte RCP_VENDOR_SPECIFIC_75 = 0x90;
        public const byte RCP_VENDOR_SPECIFIC_76 = 0x91;
        public const byte RCP_VENDOR_SPECIFIC_77 = 0x92;
        public const byte RCP_VENDOR_SPECIFIC_78 = 0x93;
        public const byte RCP_VENDOR_SPECIFIC_79 = 0x94;
        public const byte RCP_VENDOR_SPECIFIC_80 = 0x95;
        public const byte RCP_VENDOR_SPECIFIC_81 = 0x96;
        public const byte RCP_ISO_RESERVED_23 = 0x97;
        public const byte RCP_ISO_RESERVED_24 = 0x98;
        public const byte RCP_ISO_RESERVED_25 = 0x99;
        public const byte RCP_ISO_RESERVED_26 = 0x9A;
        public const byte RCP_ISO_RESERVED_27 = 0x9B;
        public const byte RCP_ISO_RESERVED_28 = 0x9C;
        public const byte RCP_ISO_RESERVED_29 = 0x9D;
        public const byte RCP_ISO_RESERVED_30 = 0x9E;
        public const byte RCP_ISO_RESERVED_31 = 0x9F;

        public const byte RDR_FW_TIME = 0xA0;               // Custom Info 
        public const byte RCP_PHY_RESERVED_05 = 0xA1;       // #define RCP_CMD_GET_LAST_RST		0xA1 // Get Last Result
        public const byte RCP_CMD_STRT_TEST_MOD = 0xA2;     // Start Test Mode
        public const byte RCP_CMD_STOP_TEST_MOD = 0xA3;     // Stop Test Mode
        public const byte RCP_CMD_STRT_RX_TEST = 0xA4;      // Start Receive Test
        public const byte RCP_CMD_STOP_RX_TEST = 0xA5;      // Stop Receive Test
        public const byte RCP_CMD_SET_MODEM_REG = 0xA6;     // Set Modem Register
        public const byte RCP_CMD_SET_RF_REG = 0xA7;        // Set RF Register
        public const byte RCP_CMD_GET_MODEM_REG = 0xA8;     // Get Modem Register
        public const byte RCP_CMD_GET_RF_REG = 0xA9;        // Get RF Register
        public const byte RCP_PHY_RESERVED_06 = 0xAA;
        public const byte RCP_PHY_RESERVED_07 = 0xAB;
        public const byte RCP_PHY_RESERVED_08 = 0xAC;
        public const byte RCP_PHY_RESERVED_09 = 0xAD;
        public const byte RCP_PHY_RESERVED_10 = 0xAE;
        public const byte RCP_PHY_RESERVED_11 = 0xAF;

        public const byte RCP_PHY_RESERVED_12 = 0xB0;
        public const byte RCP_CMD_SET_DOWNLOAD = 0xB1;      // Set Download Mode
        public const byte RCP_CMD_SET_TX_PWR_LEGACY = 0xB2;
        // public const byte RCP_CMD_GET_TX_PWR_LEGACY = 0xB3;
        public const byte RCP_CMD_SET_DUMP = 0xB3;          // Set Dump Mode
        public const byte RCP_CMD_SET_ANT_PATH = 0xB4;
        public const byte RCP_CMD_SET_LEAK_CAL_MODE = 0xB5; // #define RCP_VENDOR_SPECIFIC_97		0xB5 
        public const byte RCP_CMD_GET_IAP_VER = 0xB6;
        public const byte RCP_CMD_GET_TEMPERATURE = 0xB7;
        public const byte RCP_CMD_SET_TX_OFFSET	= 0xB8;
        public const byte RCP_CMD_GET_TX_OFFSET	= 0xB9;
        public const byte RCP_CMD_SET_TX_PWR_CL = 0xBA;
        // 0xBB is Preamble
        public const byte RCP_CMD_SET_MOD_DEPTH = 0xBC;
        public const byte RCP_CMD_SET_MODULE_TABLE = 0xBD;
        public const byte RCP_CMD_SET_DELI = 0xBE;
        public const byte RCP_CMD_SET_MOD_BLF = 0xBF;

        public const byte RCP_CMD_GET_RX_GAIN = 0xC0;       // Get Rx Gain
        public const byte RCP_CMD_SET_RX_GAIN = 0xC1;       // Set Rx Gain
        public const byte RCP_CMD_GET_PWR_CAL = 0xC2;
        public const byte RCP_CMD_SET_PWR_CAL = 0xC3;
        public const byte RCP_CMD_SET_INDUCTOR = 0xC4;      // for PRM9240AE
        public const byte RCP_CMD_GET_RSSI = 0xC5;          // Get RSSI
        public const byte RCP_CMD_SCAN_RSSI = 0xC6;         // Scan RSSI
        public const byte RCP_PHY_RESERVED_14 = 0xC7;
        public const byte RCP_CMD_SET_SAW_FILTER = 0xC8;    // for PRM9240AE
        public const byte RCP_CMD_SET_DTC = 0xC9;
        public const byte RCP_CMD_GET_DTC_RESULT = 0xCA;    // for PRM9240AE
        public const byte RCP_CMD_GET_DTC = 0xCB;           // for PRM9240AE
        public const byte RCP_PHY_RESERVED_20 = 0xCC;
        public const byte RCP_PHY_RESERVED_21 = 0xCD;
        public const byte RCP_PHY_RESERVED_22 = 0xCE;
        public const byte RCP_CMD_SET_MP_INFO = 0xCF;

        public const byte RCP_SET_INT_PARAM = 0xD0;
        public const byte RCP_CMD_SET_DEV = 0xD1;
        public const byte RCP_UPDATE_REGISTRY = 0xD2;
        public const byte RCP_ERASE_REGISTRY = 0xD3;
        public const byte RCP_GET_REGISTRY_ITEM = 0xD4;
        public const byte RCP_SET_REGISTRY_ITEM = 0xD5;
        public const byte RCP_GET_GPIO = 0xD6;
        public const byte RCP_SET_GPIO = 0xD7;
        public const byte RCP_CMD_GET_PWR_TBL = 0xD8;
        public const byte RCP_CMD_SET_PWR_TBL = 0xD9;
        public const byte RCP_CMD_GET_FB_PARAM = 0xDA;
        public const byte RCP_CMD_SET_FB_PARAM = 0xDB;
        public const byte RCP_CMD_SET_PART_NUM = 0xDC;
        public const byte RCP_CMD_GET_ADC = 0xDD;
        public const byte RCP_CMD_SET_SERIAL = 0xDE;
        public const byte RCP_MSD_DEBUG = 0xDF;

        public const byte RCP_CMD_TEST_CMD = 0xE0;
        public const byte RCP_CMD_SET_READERMODE = 0xE1;
        public const byte RCP_CMD_ATTENUATOR_CTRL = 0xE2;
        public const byte RCP_CMD_UART_BAUDRATE_CTRL = 0xE3;
        public const byte RCP_CMD_SET_OPT_FH_TABLE = 0xE4;
        public const byte RCP_CMD_GET_FH_MODE = 0xE5;
        public const byte RCP_CMD_SET_FH_MODE = 0xE6;
        public const byte RCP_CMD_GET_FH_MODE_REF_LEVEL = 0xE7;
        public const byte RCP_CMD_SET_FH_MODE_REF_LEVEL = 0xE8;

        public const byte RCP_FAIL = 0xFF;

        // RSP NACK Param
        public const byte FAIL_NONE = 0x00;
        public const byte FAIL_RDR_PWR_CTRL = 0x01;
        public const byte FAIL_RDR_CON_CTRL = 0x02;
        public const byte FAIL_RDR_GET_INFO = 0x03;
        public const byte FAIL_GET_SIG_STREN = 0x04;
        public const byte FAIL_SET_SIG_STREN = 0x05;
        public const byte FAIL_RDR_FILT_CTRLN = 0x06;
        public const byte FAIL_GET_REGION = 0x07;
        public const byte FAIL_SET_REGION = 0x08;
        public const byte FAIL_READ_TAG_MEM = 0x09;

        public const byte FAIL_READ_AUTO = 0x0A;
        public const byte FAIL_READ_AUTO_OP = 0x0B;         // Automatic Read In Operation
        public const byte FAIL_STOP_AUTO = 0x0C;            // Cannot Stop Automatic Read
        public const byte FAIL_NO_AUTO_MODE = 0x0D;         // Not in Automatic Mode
        public const byte FAIL_INVALID_PARM = 0x0E;

        public const byte FAIL_WRITE_TAG = 0x10;
        public const byte FAIL_ERASE_TAG = 0x11;
        public const byte FAIL_KILL_TAG = 0x12;
        public const byte FAIL_CTRL_LOCK = 0x13;
        public const byte FAIL_GET_LAST_RST = 0x14;
        public const byte FAIL_NO_TAG_DETECTED = 0x15;
        public const byte FAIL_PASSWORD = 0x16;
        public const byte FAIL_NOT_SUPRT_CMD = 0x17;        // Not Support CMD
        public const byte FAIL_UNDEF_CMD = 0x18;            // Not Support CMD
        public const byte FAIL_RDR_RESET = 0x19;
        public const byte FAIL_CTRL_C_PARAM = 0x1B;
        public const byte FAIL_NO_USER_DATA = 0x1C;
        public const byte FAIL_CTRL_TEST = 0x1E;

        // Vendor Specific Section
        public const byte FAIL_VENDOR_SPECIFIC_TOP = 0x30;
        public const byte FAIL_VENDOR_SPECIFIC_BOT = 0xDF;
        public const byte FAIL_RESERVED_TOP = 0xE0;
        public const byte FAIL_RESERVED_BOT = 0xFE;
        public const byte FAIL_CRC_ERROR = 0xFF;
        public const byte FAIL_CSTM_CMD = 0xFF;

        // RSP NACK Param : for 3byte format
        // 0x00 - 0x0F : EPC G2v2 Error Message
        public const byte ERR_G2V2_OTHR_ERR = 0x00;         // Ohter error
        public const byte ERR_G2V2_NOT_SUPRT = 0x01;        // Not supported
        public const byte ERR_G2V2_INSUF_PRIVIL = 0x02;     // Insufficient privileges
        public const byte ERR_G2V2_MEM_OVRRUN = 0x03;       // Memory overrun
        public const byte ERR_G2V2_MEM_LOCK = 0x04;         // Memory locked
        public const byte ERR_G2V2_CRPT_SUIT_ERR = 0x05;    // Crypto suite error
        public const byte ERR_G2V2_CMD_NOT_ENCAP = 0x06;    // Command not encapsulated
        public const byte ERR_G2V2_RESBUF_OVRFLW = 0x07;    // ResponseBuffer overflow
        public const byte ERR_G2V2_SECRT_TIMEOUT = 0x08;    // Security timeout
        public const byte ERR_G2V2_INSUF_POW = 0x0B;        // Insufficient power
        public const byte ERR_G2V2_NON_SPECF_ERR = 0x0F;    // Non-specific error
        // 0x10 - 0x7F : Vendor Specific Error
        public const byte ERR_EM_SENSR_SCHDL_CONFG = 0x11;  // Sensor Scheduling configuration
        public const byte ERR_EM_TAG_BUSY = 0x12;           // Tag busy
        public const byte ERR_EM_MEAS_TYP_NOT_SUPRT = 0x13; // Measurement type not supported
        // 0x80 - 0x8F : Protocol Error
        public const byte ERR_NO_TAG_DETECT = 0x80;
        public const byte ERR_HANDL_ACQ = 0x81;
        public const byte ERR_ACCESS_PW = 0x82;
        public const byte ERR_KILL_PW = 0x83;
        // 0x90 - 0x9F : Modem Error
        public const byte ERR_CRC = 0x90;
        public const byte ERR_RX_TIMEOUT = 0x91;
        // 0xA0 - 0xAF : Registry
        public const byte ERR_REG_UPDATE = 0xA0;
        public const byte ERR_REG_ERASE = 0xA1;
        public const byte ERR_REG_WRITE = 0xA2;
        public const byte ERR_REG_ADD_NOT_EXIST = 0xA3;
        // 0xB0 - 0xBF : Peripheral
        public const byte ERR_UART = 0xB0;
        public const byte ERR_SPI = 0xB1;
        public const byte ERR_I2C = 0xB2;
        public const byte ERR_GPIO = 0xB3;
        // 0xC0 - 0xDF : Reserved
        // 0xE0 - 0xFF : Common Error
        public const byte ERR_NOT_SUPRT_CMD = 0xE0;
        public const byte ERR_UNDF_CMD = 0xE1;
        public const byte ERR_INVLD_PARM = 0xE2;
        public const byte ERR_TOOHI_PARM = 0xE3;
        public const byte ERR_TOOLO_PARM = 0xE4;
        public const byte ERR_READ_AUTO_OP = 0xE5;
        public const byte ERR_NO_AUTO_MODE = 0xE6;
        public const byte ERR_GET_LAST_RSP = 0xE7;
        public const byte ERR_CTRL_TEST = 0xE8;
        public const byte ERR_RDR_RESET = 0xE9;
        public const byte ERR_RFIDBLK_CTRL = 0xEA;
        public const byte ERR_BUSY = 0xEB;
        public const byte ERR_CMD = 0xF0;
        public const byte ERR_VERIFY = 0xF1;
        public const byte ERR_NONE = 0xFF;
    }
}
