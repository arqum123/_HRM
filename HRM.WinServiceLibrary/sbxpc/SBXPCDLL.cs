using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace sbxpc
{
    class SBXPCDLL
    {
        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern void _DotNET();
        public static void DotNET()
        {
            _DotNET();
        }
        
        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _EnableDevice(Int32 dwMachineNumber, byte bFlag);
        public static bool EnableDevice(Int32 dwMachineNumber, byte bFlag)
        {
            return _EnableDevice(dwMachineNumber, bFlag) > 0;
        }

        #region Not Tested Part 1
        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _SetMachineType(Int32 dwMachineNumber, ref IntPtr lpszMachineType);

        public static bool _SetMachineType(Int32 dwMachineNumber, string lpszMachineType)
        {
            IntPtr string_in = Marshal.StringToBSTR(lpszMachineType);
            try
            {
                return _SetMachineType(dwMachineNumber, ref string_in) > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Marshal.FreeBSTR(string_in);
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _GetEnrollData(Int32 dwMachineNumber,Int32 dwEnrollNumber,Int32 dwEMachineNumber,Int32 dwBackupNumber, IntPtr dwMachinePrivilege, ref IntPtr dwEnrollData, IntPtr dwPassWord);

        public static bool GetEnrollData(Int32 dwMachineNumber, Int32 dwEnrollNumber, Int32 dwEMachineNumber, Int32 dwBackupNumber, out Int32 dwMachinePrivilege, byte[] dwEnrollData, out Int32 dwPassWord)
        {
            dwMachinePrivilege = 0;
            dwPassWord = 0;

            byte[] privilege = new byte[4];
            byte[] password = new byte[4];
            GCHandle gh_privilege = GCHandle.Alloc(privilege, GCHandleType.Pinned);
            GCHandle gh_password = GCHandle.Alloc(password, GCHandleType.Pinned);
            GCHandle gh_enroll_data = GCHandle.Alloc(dwEnrollData, GCHandleType.Pinned);

            try
            {
                IntPtr addr_privilege = gh_privilege.AddrOfPinnedObject();
                IntPtr addr_password = gh_password.AddrOfPinnedObject();
                IntPtr addr_enroll_data = gh_enroll_data.AddrOfPinnedObject();

                byte ret = _GetEnrollData(dwMachineNumber, dwEnrollNumber, dwEMachineNumber, 
                                        dwBackupNumber, 
                                        addr_privilege, 
                                        ref addr_enroll_data,
                                        addr_password);

                dwMachinePrivilege = new BinaryReader(new MemoryStream(privilege)).ReadInt32();
                dwPassWord = new BinaryReader(new MemoryStream(password)).ReadInt32();

                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                gh_privilege.Free();
                gh_password.Free();
                gh_enroll_data.Free();
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _GetEnrollData1(Int32 dwMachineNumber, Int32 dwEnrollNumber, Int32 dwBackupNumber, IntPtr dwMachinePrivilege, ref IntPtr dwEnrollData, IntPtr dwPassWord);

        public static bool GetEnrollData1(Int32 dwMachineNumber, Int32 dwEnrollNumber, Int32 dwBackupNumber, out Int32 dwMachinePrivilege, IntPtr dwEnrollData, out Int32 dwPassWord)
        {
            dwMachinePrivilege = 0;
            dwPassWord = 0;

            byte[] privilege = new byte[4];
            byte[] password = new byte[4];
            GCHandle gh_privilege = GCHandle.Alloc(privilege, GCHandleType.Pinned);
            GCHandle gh_password = GCHandle.Alloc(password, GCHandleType.Pinned);

            try
            {
                IntPtr addr_privilege = gh_privilege.AddrOfPinnedObject();
                IntPtr addr_password = gh_password.AddrOfPinnedObject();

                byte ret = _GetEnrollData1(dwMachineNumber, dwEnrollNumber,
                                        dwBackupNumber,
                                        addr_privilege,
                                        ref dwEnrollData,
                                        addr_password);

                 dwMachinePrivilege = new BinaryReader(new MemoryStream(privilege)).ReadInt32();
                 dwPassWord = new BinaryReader(new MemoryStream(password)).ReadInt32();

                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                gh_privilege.Free();
                gh_password.Free();
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _SetEnrollData(Int32 dwMachineNumber,Int32 dwEnrollNumber,Int32 dwEMachineNumber,Int32 dwBackupNumber, Int32 dwMachinePrivilege, ref IntPtr dwEnrollData, Int32 dwPassWord);

        public static bool SetEnrollData(Int32 dwMachineNumber, Int32 dwEnrollNumber, Int32 dwEMachineNumber, Int32 dwBackupNumber, Int32 dwMachinePrivilege, byte[] dwEnrollData, Int32 dwPassWord)
        {
            GCHandle gh_enroll_data = GCHandle.Alloc(dwEnrollData, GCHandleType.Pinned);
            try
            {
                IntPtr addr = gh_enroll_data.AddrOfPinnedObject();
                byte ret = _SetEnrollData(dwMachineNumber, dwEnrollNumber, dwEMachineNumber,
                                        dwBackupNumber,
                                        dwMachinePrivilege,
                                        ref addr,
                                        dwPassWord);

                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally { gh_enroll_data.Free(); }
        }
        #endregion

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _SetEnrollData1(Int32 dwMachineNumber, Int32 dwEnrollNumber, Int32 dwBackupNumber, Int32 dwMachinePrivilege, ref IntPtr dwEnrollData, Int32 dwPassWord);

        public static bool SetEnrollData1(Int32 dwMachineNumber, Int32 dwEnrollNumber, Int32 dwBackupNumber, Int32 dwMachinePrivilege, IntPtr dwEnrollData, Int32 dwPassWord)
        {
            try
            {
                byte ret = _SetEnrollData1(dwMachineNumber, dwEnrollNumber,
                                        dwBackupNumber,
                                        dwMachinePrivilege,
                                        ref dwEnrollData,
                                        dwPassWord);

                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #region Not Tested Part 2
        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _DeleteEnrollData(Int32 dwMachineNumber,Int32 dwEnrollNumber,Int32 dwEMachineNumber,Int32 dwBackupNumber);

        public static bool DeleteEnrollData(Int32 dwMachineNumber, Int32 dwEnrollNumber, Int32 dwEMachineNumber, Int32 dwBackupNumber)
        {
            return _DeleteEnrollData(dwMachineNumber, dwEnrollNumber, dwEMachineNumber, dwBackupNumber) > 0;
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _ReadSuperLogData(Int32 dwMachineNumber, byte bReadMark);
        public static bool ReadSuperLogData(Int32 dwMachineNumber, byte bReadMark)
        {
            return _ReadSuperLogData(dwMachineNumber, bReadMark) > 0;
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _GetSuperLogData(Int32 dwMachineNumber, IntPtr dwTMachineNumber, IntPtr dwSEnrollNumber, IntPtr dwSMachineNumber, IntPtr dwGEnrollNumber, IntPtr dwGMachineNumber, IntPtr dwManipulation, IntPtr dwBackupNumber, IntPtr dwYear, IntPtr dwMonth, IntPtr dwDay, IntPtr dwHour, IntPtr dwMinute, IntPtr dwSecond);

        public static bool GetSuperLogData(Int32 dwMachineNumber, out Int32 dwTMachineNumber, out Int32 dwSEnrollNumber, out Int32 dwSMachineNumber, out Int32 dwGEnrollNumber, out Int32 dwGMachineNumber, out Int32 dwManipulation, out Int32 dwBackupNumber, out Int32 dwYear, out Int32 dwMonth, out Int32 dwDay, out Int32 dwHour, out Int32 dwMinute, out Int32 dwSecond)
        {
            dwTMachineNumber = 0;
            dwSEnrollNumber = 0;
            dwSMachineNumber = 0;
            dwGEnrollNumber = 0;
            dwGMachineNumber = 0;
            dwManipulation = 0;
            dwBackupNumber = 0;
            dwYear = 0;
            dwMonth = 0;
            dwDay = 0;
            dwHour = 0;
            dwMinute = 0;
            dwSecond = 0;

            byte[] bin_dwTMachineNumber = new byte[4];
            byte[] bin_dwSEnrollNumber = new byte[4];
            byte[] bin_dwSMachineNumber = new byte[4];
            byte[] bin_dwGEnrollNumber = new byte[4];
            byte[] bin_dwGMachineNumber = new byte[4];
            byte[] bin_dwManipulation = new byte[4];
            byte[] bin_dwBackupNumber = new byte[4];
            byte[] bin_dwYear = new byte[4];
            byte[] bin_dwMonth = new byte[4];
            byte[] bin_dwDay = new byte[4];
            byte[] bin_dwHour = new byte[4];
            byte[] bin_dwMinute = new byte[4];
            byte[] bin_dwSecond = new byte[4];

            GCHandle gh_dwTMachineNumber = GCHandle.Alloc(bin_dwTMachineNumber, GCHandleType.Pinned);
            GCHandle gh_dwSEnrollNumber = GCHandle.Alloc(bin_dwSEnrollNumber, GCHandleType.Pinned);
            GCHandle gh_dwSMachineNumber = GCHandle.Alloc(bin_dwSMachineNumber, GCHandleType.Pinned);
            GCHandle gh_dwGEnrollNumber = GCHandle.Alloc(bin_dwGEnrollNumber, GCHandleType.Pinned);
            GCHandle gh_dwGMachineNumber = GCHandle.Alloc(bin_dwGMachineNumber, GCHandleType.Pinned);
            GCHandle gh_dwManipulation = GCHandle.Alloc(bin_dwManipulation, GCHandleType.Pinned);
            GCHandle gh_dwBackupNumber = GCHandle.Alloc(bin_dwBackupNumber, GCHandleType.Pinned);
            GCHandle gh_dwYear = GCHandle.Alloc(bin_dwYear, GCHandleType.Pinned);
            GCHandle gh_dwMonth = GCHandle.Alloc(bin_dwMonth, GCHandleType.Pinned);
            GCHandle gh_dwDay = GCHandle.Alloc(bin_dwDay, GCHandleType.Pinned);
            GCHandle gh_dwHour = GCHandle.Alloc(bin_dwHour, GCHandleType.Pinned);
            GCHandle gh_dwMinute = GCHandle.Alloc(bin_dwMinute, GCHandleType.Pinned);
            GCHandle gh_dwSecond = GCHandle.Alloc(bin_dwSecond, GCHandleType.Pinned);
            try
            {
                IntPtr addr_dwTMachineNumber   = gh_dwTMachineNumber.AddrOfPinnedObject();
                IntPtr addr_dwSEnrollNumber    = gh_dwSEnrollNumber.AddrOfPinnedObject();
                IntPtr addr_dwSMachineNumber   = gh_dwSMachineNumber.AddrOfPinnedObject();
                IntPtr addr_dwGEnrollNumber    = gh_dwGEnrollNumber.AddrOfPinnedObject();
                IntPtr addr_dwGMachineNumber   = gh_dwGMachineNumber.AddrOfPinnedObject();
                IntPtr addr_dwManipulation     = gh_dwManipulation.AddrOfPinnedObject();
                IntPtr addr_dwBackupNumber     = gh_dwBackupNumber.AddrOfPinnedObject();
                IntPtr addr_dwYear             = gh_dwYear.AddrOfPinnedObject();
                IntPtr addr_dwMonth            = gh_dwMonth.AddrOfPinnedObject();
                IntPtr addr_dwDay              = gh_dwDay.AddrOfPinnedObject();
                IntPtr addr_dwHour             = gh_dwHour.AddrOfPinnedObject();
                IntPtr addr_dwMinute           = gh_dwMinute.AddrOfPinnedObject();
                IntPtr addr_dwSecond           = gh_dwSecond.AddrOfPinnedObject();

                byte ret = _GetSuperLogData(dwMachineNumber,
                                        addr_dwTMachineNumber,
                                        addr_dwSEnrollNumber,
                                        addr_dwSMachineNumber,
                                        addr_dwGEnrollNumber,
                                        addr_dwGMachineNumber,
                                        addr_dwManipulation,
                                        addr_dwBackupNumber,
                                        addr_dwYear,
                                        addr_dwMonth,
                                        addr_dwDay,
                                        addr_dwHour,
                                        addr_dwMinute,
                                        addr_dwSecond);

                dwTMachineNumber   = new BinaryReader(new MemoryStream(bin_dwTMachineNumber)).ReadInt32();
                dwSEnrollNumber    = new BinaryReader(new MemoryStream(bin_dwSEnrollNumber)).ReadInt32();
                dwSMachineNumber   = new BinaryReader(new MemoryStream(bin_dwSMachineNumber)).ReadInt32();
                dwGEnrollNumber    = new BinaryReader(new MemoryStream(bin_dwGEnrollNumber)).ReadInt32();
                dwGMachineNumber   = new BinaryReader(new MemoryStream(bin_dwGMachineNumber)).ReadInt32();
                dwManipulation     = new BinaryReader(new MemoryStream(bin_dwManipulation)).ReadInt32();
                dwBackupNumber     = new BinaryReader(new MemoryStream(bin_dwBackupNumber)).ReadInt32();
                dwYear             = new BinaryReader(new MemoryStream(bin_dwYear)).ReadInt32();
                dwMonth            = new BinaryReader(new MemoryStream(bin_dwMonth)).ReadInt32();
                dwDay              = new BinaryReader(new MemoryStream(bin_dwDay)).ReadInt32();
                dwHour             = new BinaryReader(new MemoryStream(bin_dwHour)).ReadInt32();
                dwMinute           = new BinaryReader(new MemoryStream(bin_dwMinute)).ReadInt32();
                dwSecond           = new BinaryReader(new MemoryStream(bin_dwSecond)).ReadInt32();

                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                gh_dwTMachineNumber.Free();
                gh_dwSEnrollNumber.Free();
                gh_dwSMachineNumber.Free();
                gh_dwGEnrollNumber.Free();
                gh_dwGMachineNumber.Free();
                gh_dwManipulation.Free();
                gh_dwBackupNumber.Free();
                gh_dwYear.Free();
                gh_dwMonth.Free();
                gh_dwDay.Free();
                gh_dwHour.Free();
                gh_dwMinute.Free();
                gh_dwSecond.Free();
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _ReadGeneralLogData(Int32 dwMachineNumber, byte bReadMark);

        public static bool ReadGeneralLogData(Int32 dwMachineNumber, byte bReadMark)
        {
            return _ReadGeneralLogData(dwMachineNumber, bReadMark) > 0;
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _GetGeneralLogData(Int32 dwMachineNumber, IntPtr dwTMachineNumber, IntPtr dwEnrollNumber, IntPtr dwEMachineNumber, IntPtr dwVerifyMode, IntPtr dwYear, IntPtr dwMonth, IntPtr dwDay, IntPtr dwHour, IntPtr dwMinute, IntPtr dwSecond);

        public static bool GetGeneralLogData(Int32 dwMachineNumber, out Int32 dwTMachineNumber, out Int32 dwEnrollNumber, out Int32 dwEMachineNumber, out Int32 dwVerifyMode, out Int32 dwYear, out Int32 dwMonth, out Int32 dwDay, out Int32 dwHour, out Int32 dwMinute, out Int32 dwSecond)
        {
            dwTMachineNumber = 0;
            dwEnrollNumber = 0;
            dwEMachineNumber = 0;
            dwVerifyMode = 0;
            dwYear = 0;
            dwMonth = 0;
            dwDay = 0;
            dwHour = 0;
            dwMinute = 0;
            dwSecond = 0;

            byte[] bin_dwTMachineNumber = new byte[4];
            byte[] bin_dwEnrollNumber = new byte[4];
            byte[] bin_dwEMachineNumber = new byte[4];
            byte[] bin_dwVerifyMode = new byte[4];
            byte[] bin_dwYear = new byte[4];
            byte[] bin_dwMonth = new byte[4];
            byte[] bin_dwDay = new byte[4];
            byte[] bin_dwHour = new byte[4];
            byte[] bin_dwMinute = new byte[4];
            byte[] bin_dwSecond = new byte[4];

            GCHandle gh_dwTMachineNumber = GCHandle.Alloc(bin_dwTMachineNumber, GCHandleType.Pinned);
            GCHandle gh_dwEnrollNumber = GCHandle.Alloc(bin_dwEnrollNumber, GCHandleType.Pinned);
            GCHandle gh_dwEMachineNumber = GCHandle.Alloc(bin_dwEMachineNumber, GCHandleType.Pinned);
            GCHandle gh_dwVerifyMode = GCHandle.Alloc(bin_dwVerifyMode, GCHandleType.Pinned);
            GCHandle gh_dwYear = GCHandle.Alloc(bin_dwYear, GCHandleType.Pinned);
            GCHandle gh_dwMonth = GCHandle.Alloc(bin_dwMonth, GCHandleType.Pinned);
            GCHandle gh_dwDay = GCHandle.Alloc(bin_dwDay, GCHandleType.Pinned);
            GCHandle gh_dwHour = GCHandle.Alloc(bin_dwHour, GCHandleType.Pinned);
            GCHandle gh_dwMinute = GCHandle.Alloc(bin_dwMinute, GCHandleType.Pinned);
            GCHandle gh_dwSecond = GCHandle.Alloc(bin_dwSecond, GCHandleType.Pinned);

            try
            {
                IntPtr addr_dwTMachineNumber = gh_dwTMachineNumber.AddrOfPinnedObject();
                IntPtr addr_dwEnrollNumber = gh_dwEnrollNumber.AddrOfPinnedObject();
                IntPtr addr_dwEMachineNumber = gh_dwEMachineNumber.AddrOfPinnedObject();
                IntPtr addr_dwVerifyMode = gh_dwVerifyMode.AddrOfPinnedObject();
                IntPtr addr_dwYear = gh_dwYear.AddrOfPinnedObject();
                IntPtr addr_dwMonth = gh_dwMonth.AddrOfPinnedObject();
                IntPtr addr_dwDay = gh_dwDay.AddrOfPinnedObject();
                IntPtr addr_dwHour = gh_dwHour.AddrOfPinnedObject();
                IntPtr addr_dwMinute = gh_dwMinute.AddrOfPinnedObject();
                IntPtr addr_dwSecond = gh_dwSecond.AddrOfPinnedObject();

                byte ret = _GetGeneralLogData(dwMachineNumber,
                                addr_dwTMachineNumber,
                                addr_dwEnrollNumber,
                                addr_dwEMachineNumber,
                                addr_dwVerifyMode,
                                addr_dwYear,
                                addr_dwMonth,
                                addr_dwDay,
                                addr_dwHour,
                                addr_dwMinute,
                                addr_dwSecond);

                dwTMachineNumber = new BinaryReader(new MemoryStream(bin_dwTMachineNumber)).ReadInt32();
                dwEnrollNumber = new BinaryReader(new MemoryStream(bin_dwEnrollNumber)).ReadInt32();
                dwEMachineNumber = new BinaryReader(new MemoryStream(bin_dwEMachineNumber)).ReadInt32();
                dwVerifyMode = new BinaryReader(new MemoryStream(bin_dwVerifyMode)).ReadInt32();
                dwYear = new BinaryReader(new MemoryStream(bin_dwYear)).ReadInt32();
                dwMonth = new BinaryReader(new MemoryStream(bin_dwMonth)).ReadInt32();
                dwDay = new BinaryReader(new MemoryStream(bin_dwDay)).ReadInt32();
                dwHour = new BinaryReader(new MemoryStream(bin_dwHour)).ReadInt32();
                dwMinute = new BinaryReader(new MemoryStream(bin_dwMinute)).ReadInt32();
                dwSecond = new BinaryReader(new MemoryStream(bin_dwSecond)).ReadInt32();

                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                gh_dwTMachineNumber.Free();
                gh_dwEnrollNumber.Free();
                gh_dwEMachineNumber.Free();
                gh_dwVerifyMode.Free();
                gh_dwYear.Free();
                gh_dwMonth.Free();
                gh_dwDay.Free();
                gh_dwHour.Free();
                gh_dwMinute.Free();
                gh_dwSecond.Free();
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _ReadAllSLogData(Int32 dwMachineNumber);

        public static bool ReadAllSLogData(Int32 dwMachineNumber)
        {
            return _ReadAllSLogData(dwMachineNumber) > 0;
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _GetAllSLogData(Int32 dwMachineNumber, IntPtr dwTMachineNumber, IntPtr dwSEnrollNumber, IntPtr dwSMachineNumber, IntPtr dwGEnrollNumber, IntPtr dwGMachineNumber, IntPtr dwManipulation, IntPtr dwBackupNumber, IntPtr dwYear, IntPtr dwMonth, IntPtr dwDay, IntPtr dwHour, IntPtr dwMinute, IntPtr dwSecond);

        public static bool GetAllSLogData(Int32 dwMachineNumber, out Int32 dwTMachineNumber, out Int32 dwSEnrollNumber, out Int32 dwSMachineNumber, out Int32 dwGEnrollNumber, out Int32 dwGMachineNumber, out Int32 dwManipulation, out Int32 dwBackupNumber, out Int32 dwYear, out Int32 dwMonth, out Int32 dwDay, out Int32 dwHour, out Int32 dwMinute, out Int32 dwSecond)
        {
            dwTMachineNumber = 0;
            dwSEnrollNumber = 0;
            dwSMachineNumber = 0;
            dwGEnrollNumber = 0;
            dwGMachineNumber = 0;
            dwManipulation = 0;
            dwBackupNumber = 0;
            dwYear = 0;
            dwMonth = 0;
            dwDay = 0;
            dwHour = 0;
            dwMinute = 0;
            dwSecond = 0;

            byte[] bin_dwTMachineNumber = new byte[4];
            byte[] bin_dwSEnrollNumber = new byte[4];
            byte[] bin_dwSMachineNumber = new byte[4];
            byte[] bin_dwGEnrollNumber = new byte[4];
            byte[] bin_dwGMachineNumber = new byte[4];
            byte[] bin_dwManipulation = new byte[4];
            byte[] bin_dwBackupNumber = new byte[4];
            byte[] bin_dwYear = new byte[4];
            byte[] bin_dwMonth = new byte[4];
            byte[] bin_dwDay = new byte[4];
            byte[] bin_dwHour = new byte[4];
            byte[] bin_dwMinute = new byte[4];
            byte[] bin_dwSecond = new byte[4];

            GCHandle gh_dwTMachineNumber = GCHandle.Alloc(bin_dwTMachineNumber, GCHandleType.Pinned);
            GCHandle gh_dwSEnrollNumber = GCHandle.Alloc(bin_dwSEnrollNumber, GCHandleType.Pinned);
            GCHandle gh_dwSMachineNumber = GCHandle.Alloc(bin_dwSMachineNumber, GCHandleType.Pinned);
            GCHandle gh_dwGEnrollNumber = GCHandle.Alloc(bin_dwGEnrollNumber, GCHandleType.Pinned);
            GCHandle gh_dwGMachineNumber = GCHandle.Alloc(bin_dwGMachineNumber, GCHandleType.Pinned);
            GCHandle gh_dwManipulation = GCHandle.Alloc(bin_dwManipulation, GCHandleType.Pinned);
            GCHandle gh_dwBackupNumber = GCHandle.Alloc(bin_dwBackupNumber, GCHandleType.Pinned);
            GCHandle gh_dwYear = GCHandle.Alloc(bin_dwYear, GCHandleType.Pinned);
            GCHandle gh_dwMonth = GCHandle.Alloc(bin_dwMonth, GCHandleType.Pinned);
            GCHandle gh_dwDay = GCHandle.Alloc(bin_dwDay, GCHandleType.Pinned);
            GCHandle gh_dwHour = GCHandle.Alloc(bin_dwHour, GCHandleType.Pinned);
            GCHandle gh_dwMinute = GCHandle.Alloc(bin_dwMinute, GCHandleType.Pinned);
            GCHandle gh_dwSecond = GCHandle.Alloc(bin_dwSecond, GCHandleType.Pinned);

            try
            {
                IntPtr addr_dwTMachineNumber   = gh_dwTMachineNumber.AddrOfPinnedObject();
                IntPtr addr_dwSEnrollNumber    = gh_dwSEnrollNumber.AddrOfPinnedObject();
                IntPtr addr_dwSMachineNumber   = gh_dwSMachineNumber.AddrOfPinnedObject();
                IntPtr addr_dwGEnrollNumber    = gh_dwGEnrollNumber.AddrOfPinnedObject();
                IntPtr addr_dwGMachineNumber   = gh_dwGMachineNumber.AddrOfPinnedObject();
                IntPtr addr_dwManipulation     = gh_dwManipulation.AddrOfPinnedObject();
                IntPtr addr_dwBackupNumber     = gh_dwBackupNumber.AddrOfPinnedObject();
                IntPtr addr_dwYear             = gh_dwYear.AddrOfPinnedObject();
                IntPtr addr_dwMonth            = gh_dwMonth.AddrOfPinnedObject();
                IntPtr addr_dwDay              = gh_dwDay.AddrOfPinnedObject();
                IntPtr addr_dwHour             = gh_dwHour.AddrOfPinnedObject();
                IntPtr addr_dwMinute           = gh_dwMinute.AddrOfPinnedObject();
                IntPtr addr_dwSecond           = gh_dwSecond.AddrOfPinnedObject();

                byte ret = _GetAllSLogData(dwMachineNumber,
                                        addr_dwTMachineNumber,
                                        addr_dwSEnrollNumber,
                                        addr_dwSMachineNumber,
                                        addr_dwGEnrollNumber,
                                        addr_dwGMachineNumber,
                                        addr_dwManipulation,
                                        addr_dwBackupNumber,
                                        addr_dwYear,
                                        addr_dwMonth,
                                        addr_dwDay,
                                        addr_dwHour,
                                        addr_dwMinute,
                                        addr_dwSecond);

                dwTMachineNumber   = new BinaryReader(new MemoryStream(bin_dwTMachineNumber)).ReadInt32();
                dwSEnrollNumber    = new BinaryReader(new MemoryStream(bin_dwSEnrollNumber)).ReadInt32();
                dwSMachineNumber   = new BinaryReader(new MemoryStream(bin_dwSMachineNumber)).ReadInt32();
                dwGEnrollNumber    = new BinaryReader(new MemoryStream(bin_dwGEnrollNumber)).ReadInt32();
                dwGMachineNumber   = new BinaryReader(new MemoryStream(bin_dwGMachineNumber)).ReadInt32();
                dwManipulation     = new BinaryReader(new MemoryStream(bin_dwManipulation)).ReadInt32();
                dwBackupNumber     = new BinaryReader(new MemoryStream(bin_dwBackupNumber)).ReadInt32();
                dwYear             = new BinaryReader(new MemoryStream(bin_dwYear)).ReadInt32();
                dwMonth            = new BinaryReader(new MemoryStream(bin_dwMonth)).ReadInt32();
                dwDay              = new BinaryReader(new MemoryStream(bin_dwDay)).ReadInt32();
                dwHour             = new BinaryReader(new MemoryStream(bin_dwHour)).ReadInt32();
                dwMinute           = new BinaryReader(new MemoryStream(bin_dwMinute)).ReadInt32();
                dwSecond           = new BinaryReader(new MemoryStream(bin_dwSecond)).ReadInt32();

                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                gh_dwTMachineNumber.Free();
                gh_dwSEnrollNumber.Free();
                gh_dwSMachineNumber.Free();
                gh_dwGEnrollNumber.Free();
                gh_dwGMachineNumber.Free();
                gh_dwManipulation.Free();
                gh_dwBackupNumber.Free();
                gh_dwYear.Free();
                gh_dwMonth.Free();
                gh_dwDay.Free();
                gh_dwHour.Free();
                gh_dwMinute.Free();
                gh_dwSecond.Free();
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _ReadAllGLogData(Int32 dwMachineNumber);

        public static bool ReadAllGLogData(Int32 dwMachineNumber)
        {
            return _ReadAllGLogData(dwMachineNumber) > 0;
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _GetAllGLogData(Int32 dwMachineNumber, IntPtr dwTMachineNumber, IntPtr dwEnrollNumber, IntPtr dwEMachineNumber, IntPtr dwVerifyMode, IntPtr dwYear, IntPtr dwMonth, IntPtr dwDay, IntPtr dwHour, IntPtr dwMinute, IntPtr dwSecond);

        public static bool GetAllGLogData(Int32 dwMachineNumber, out Int32 dwTMachineNumber, out Int32 dwEnrollNumber, out Int32 dwEMachineNumber, out Int32 dwVerifyMode, out Int32 dwYear, out Int32 dwMonth, out Int32 dwDay, out Int32 dwHour, out Int32 dwMinute, out Int32 dwSecond)
        {
            dwTMachineNumber = 0;
            dwEnrollNumber = 0;
            dwEMachineNumber = 0;
            dwVerifyMode = 0;
            dwYear = 0;
            dwMonth = 0;
            dwDay = 0;
            dwHour = 0;
            dwMinute = 0;
            dwSecond = 0;

            byte[] bin_dwTMachineNumber = new byte[4];
            byte[] bin_dwEnrollNumber = new byte[4];
            byte[] bin_dwEMachineNumber = new byte[4];
            byte[] bin_dwVerifyMode = new byte[4];
            byte[] bin_dwYear = new byte[4];
            byte[] bin_dwMonth = new byte[4];
            byte[] bin_dwDay = new byte[4];
            byte[] bin_dwHour = new byte[4];
            byte[] bin_dwMinute = new byte[4];
            byte[] bin_dwSecond = new byte[4];

            GCHandle gh_dwTMachineNumber = GCHandle.Alloc(bin_dwTMachineNumber, GCHandleType.Pinned);
            GCHandle gh_dwEnrollNumber = GCHandle.Alloc(bin_dwEnrollNumber, GCHandleType.Pinned);
            GCHandle gh_dwEMachineNumber = GCHandle.Alloc(bin_dwEMachineNumber, GCHandleType.Pinned);
            GCHandle gh_dwVerifyMode = GCHandle.Alloc(bin_dwVerifyMode, GCHandleType.Pinned);
            GCHandle gh_dwYear = GCHandle.Alloc(bin_dwYear, GCHandleType.Pinned);
            GCHandle gh_dwMonth = GCHandle.Alloc(bin_dwMonth, GCHandleType.Pinned);
            GCHandle gh_dwDay = GCHandle.Alloc(bin_dwDay, GCHandleType.Pinned);
            GCHandle gh_dwHour = GCHandle.Alloc(bin_dwHour, GCHandleType.Pinned);
            GCHandle gh_dwMinute = GCHandle.Alloc(bin_dwMinute, GCHandleType.Pinned);
            GCHandle gh_dwSecond = GCHandle.Alloc(bin_dwSecond, GCHandleType.Pinned);

            try
            {
                IntPtr addr_dwTMachineNumber = gh_dwTMachineNumber.AddrOfPinnedObject();
                IntPtr addr_dwEnrollNumber = gh_dwEnrollNumber.AddrOfPinnedObject();
                IntPtr addr_dwEMachineNumber = gh_dwEMachineNumber.AddrOfPinnedObject();
                IntPtr addr_dwVerifyMode = gh_dwVerifyMode.AddrOfPinnedObject();
                IntPtr addr_dwYear = gh_dwYear.AddrOfPinnedObject();
                IntPtr addr_dwMonth = gh_dwMonth.AddrOfPinnedObject();
                IntPtr addr_dwDay = gh_dwDay.AddrOfPinnedObject();
                IntPtr addr_dwHour = gh_dwHour.AddrOfPinnedObject();
                IntPtr addr_dwMinute = gh_dwMinute.AddrOfPinnedObject();
                IntPtr addr_dwSecond = gh_dwSecond.AddrOfPinnedObject();

                byte ret = _GetAllGLogData(dwMachineNumber,
                                addr_dwTMachineNumber,
                                addr_dwEnrollNumber,
                                addr_dwEMachineNumber,
                                addr_dwVerifyMode,
                                addr_dwYear,
                                addr_dwMonth,
                                addr_dwDay,
                                addr_dwHour,
                                addr_dwMinute,
                                addr_dwSecond);

                dwTMachineNumber = new BinaryReader(new MemoryStream(bin_dwTMachineNumber)).ReadInt32();
                dwEnrollNumber = new BinaryReader(new MemoryStream(bin_dwEnrollNumber)).ReadInt32();
                dwEMachineNumber = new BinaryReader(new MemoryStream(bin_dwEMachineNumber)).ReadInt32();
                dwVerifyMode = new BinaryReader(new MemoryStream(bin_dwVerifyMode)).ReadInt32();
                dwYear = new BinaryReader(new MemoryStream(bin_dwYear)).ReadInt32();
                dwMonth = new BinaryReader(new MemoryStream(bin_dwMonth)).ReadInt32();
                dwDay = new BinaryReader(new MemoryStream(bin_dwDay)).ReadInt32();
                dwHour = new BinaryReader(new MemoryStream(bin_dwHour)).ReadInt32();
                dwMinute = new BinaryReader(new MemoryStream(bin_dwMinute)).ReadInt32();
                dwSecond = new BinaryReader(new MemoryStream(bin_dwSecond)).ReadInt32();

                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                gh_dwTMachineNumber.Free();
                gh_dwEnrollNumber.Free();
                gh_dwEMachineNumber.Free();
                gh_dwVerifyMode.Free();
                gh_dwYear.Free();
                gh_dwMonth.Free();
                gh_dwDay.Free();
                gh_dwHour.Free();
                gh_dwMinute.Free();
                gh_dwSecond.Free();
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _GetDeviceStatus(Int32 dwMachineNumber, Int32 dwStatus, IntPtr dwValue);

        public static bool GetDeviceStatus(Int32 dwMachineNumber, Int32 dwStatus, out UInt32 dwValue)
        {
            dwValue = 0;

            byte[] bin_dwValue = new byte[4];

            GCHandle gh_dwValue = GCHandle.Alloc(bin_dwValue, GCHandleType.Pinned);

            try
            {
                IntPtr addr_dwValue = gh_dwValue.AddrOfPinnedObject();

                byte ret = _GetDeviceStatus(dwMachineNumber,
                                dwStatus,
                                addr_dwValue);

                dwValue = new BinaryReader(new MemoryStream(bin_dwValue)).ReadUInt32();

                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                gh_dwValue.Free();
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _GetDeviceInfo(Int32 dwMachineNumber, Int32 dwInfo, IntPtr dwValue);

        public static bool GetDeviceInfo(Int32 dwMachineNumber, Int32 dwInfo, out UInt32 dwValue)
        {
            dwValue = 0;

            byte[] bin_dwValue = new byte[4];

            GCHandle gh_dwValue = GCHandle.Alloc(bin_dwValue, GCHandleType.Pinned);
            try
            {
                IntPtr addr_dwValue = gh_dwValue.AddrOfPinnedObject();

                byte ret = _GetDeviceInfo(dwMachineNumber,
                                dwInfo,
                                addr_dwValue);

                dwValue = new BinaryReader(new MemoryStream(bin_dwValue)).ReadUInt32();

                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                gh_dwValue.Free();
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _SetDeviceInfo(Int32 dwMachineNumber, Int32 dwInfo, Int32 dwValue);

        public static bool SetDeviceInfo(Int32 dwMachineNumber, Int32 dwInfo, Int32 dwValue)
        {
            return _SetDeviceInfo(dwMachineNumber, dwInfo, dwValue) > 0;
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _GetDeviceTime(Int32 dwMachineNumber, IntPtr dwYear, IntPtr dwMonth, IntPtr dwDay, IntPtr dwHour, IntPtr dwMinute, IntPtr dwSecond, IntPtr dwDayOfWeek);

        public static bool GetDeviceTime(Int32 dwMachineNumber, out Int32 dwYear, out Int32 dwMonth, out Int32 dwDay, out Int32 dwHour, out Int32 dwMinute, out Int32 dwSecond, out Int32 dwDayOfWeek)
        {
            dwYear = 0;
            dwMonth = 0;
            dwDay = 0;
            dwHour = 0;
            dwMinute = 0;
            dwSecond = 0;
            dwDayOfWeek = 0;

            byte[] bin_dwYear = new byte[4];
            byte[] bin_dwMonth = new byte[4];
            byte[] bin_dwDay = new byte[4];
            byte[] bin_dwHour = new byte[4];
            byte[] bin_dwMinute = new byte[4];
            byte[] bin_dwSecond = new byte[4];
            byte[] bin_dwDayOfWeek = new byte[4];

            GCHandle gh_dwYear = GCHandle.Alloc(bin_dwYear, GCHandleType.Pinned);
            GCHandle gh_dwMonth = GCHandle.Alloc(bin_dwMonth, GCHandleType.Pinned);
            GCHandle gh_dwDay = GCHandle.Alloc(bin_dwDay, GCHandleType.Pinned);
            GCHandle gh_dwHour = GCHandle.Alloc(bin_dwHour, GCHandleType.Pinned);
            GCHandle gh_dwMinute = GCHandle.Alloc(bin_dwMinute, GCHandleType.Pinned);
            GCHandle gh_dwSecond = GCHandle.Alloc(bin_dwSecond, GCHandleType.Pinned);
            GCHandle gh_dwDayOfWeek = GCHandle.Alloc(bin_dwDayOfWeek, GCHandleType.Pinned);

            try
            {
                IntPtr addr_dwYear = gh_dwYear.AddrOfPinnedObject();
                IntPtr addr_dwMonth = gh_dwMonth.AddrOfPinnedObject();
                IntPtr addr_dwDay = gh_dwDay.AddrOfPinnedObject();
                IntPtr addr_dwHour = gh_dwHour.AddrOfPinnedObject();
                IntPtr addr_dwMinute = gh_dwMinute.AddrOfPinnedObject();
                IntPtr addr_dwSecond = gh_dwSecond.AddrOfPinnedObject();         
                IntPtr addr_dwDayOfWeek = gh_dwDayOfWeek.AddrOfPinnedObject();

                byte ret = _GetDeviceTime(dwMachineNumber,
                                        addr_dwYear,    
                                        addr_dwMonth,    
                                        addr_dwDay,      
                                        addr_dwHour,     
                                        addr_dwMinute,
                                        addr_dwSecond,   
                                        addr_dwDayOfWeek);

                dwYear = new BinaryReader(new MemoryStream(bin_dwYear)).ReadInt32();
                dwMonth = new BinaryReader(new MemoryStream(bin_dwMonth)).ReadInt32();
                dwDay = new BinaryReader(new MemoryStream(bin_dwDay)).ReadInt32();
                dwHour = new BinaryReader(new MemoryStream(bin_dwHour)).ReadInt32();
                dwMinute = new BinaryReader(new MemoryStream(bin_dwMinute)).ReadInt32();
                dwSecond = new BinaryReader(new MemoryStream(bin_dwSecond)).ReadInt32();
                dwDayOfWeek = new BinaryReader(new MemoryStream(bin_dwDayOfWeek)).ReadInt32();

                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                gh_dwYear.Free();
                gh_dwMonth.Free();
                gh_dwDay.Free();
                gh_dwHour.Free();
                gh_dwMinute.Free();
                gh_dwSecond.Free();
                gh_dwDayOfWeek.Free();
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _SetDeviceTime(Int32 dwMachineNumber);

        public static bool SetDeviceTime(Int32 dwMachineNumber)
        {
            return _SetDeviceTime(dwMachineNumber) > 0;
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _SetDeviceTime1(Int32 dwMachineNumber,Int32 dwYear,Int32 dwMonth,Int32 dwDay,Int32 dwHour,Int32 dwMinute,Int32 dwSecond);

        public static bool SetDeviceTime1(Int32 dwMachineNumber, Int32 dwYear, Int32 dwMonth, Int32 dwDay, Int32 dwHour, Int32 dwMinute, Int32 dwSecond)
        {
            return _SetDeviceTime1(dwMachineNumber, dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond) > 0;
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _EnableUser(Int32 dwMachineNumber, Int32 dwEnrollNumber, Int32 dwEMachineNumber, Int32 dwBackupNumber, byte bFlag);

        public static bool EnableUser(Int32 dwMachineNumber, Int32 dwEnrollNumber, Int32 dwEMachineNumber, Int32 dwBackupNumber, byte bFlag)
        {
            return _EnableUser(dwMachineNumber, dwEnrollNumber, dwEMachineNumber, dwBackupNumber, bFlag) > 0;
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _PowerOnAllDevice(Int32 dwMachineNumber);

        public static bool PowerOnAllDevice(Int32 dwMachineNumber)
        {
            return _PowerOnAllDevice(dwMachineNumber) > 0;
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _PowerOffDevice(Int32 dwMachineNumber);

        public static bool PowerOffDevice(Int32 dwMachineNumber)
        {
            return _PowerOffDevice(dwMachineNumber) > 0;
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _ModifyPrivilege(Int32 dwMachineNumber, Int32 dwEnrollNumber, Int32 dwEMachineNumber, Int32 dwBackupNumber, Int32 dwMachinePrivilege);

        public static bool ModifyPrivilege(Int32 dwMachineNumber, Int32 dwEnrollNumber, Int32 dwEMachineNumber, Int32 dwBackupNumber, Int32 dwMachinePrivilege)
        {
            return _ModifyPrivilege(dwMachineNumber, dwEnrollNumber, dwEMachineNumber, dwBackupNumber, dwMachinePrivilege) > 0;
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _ReadAllUserID(Int32 dwMachineNumber);

        public static bool ReadAllUserID(Int32 dwMachineNumber)
        {
            return _ReadAllUserID(dwMachineNumber) > 0;
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _GetAllUserID(Int32 dwMachineNumber, IntPtr dwEnrollNumber, IntPtr dwEMachineNumber, IntPtr dwBackupNumber, IntPtr dwMachinePrivilege, IntPtr dwEnable);

        public static bool GetAllUserID(Int32 dwMachineNumber, out Int32 dwEnrollNumber, out Int32 dwEMachineNumber, out Int32 dwBackupNumber, out Int32 dwMachinePrivilege, out Int32 dwEnable)
        {
            dwEnrollNumber = 0;
            dwEMachineNumber = 0;
            dwBackupNumber = 0;
            dwMachinePrivilege = 0;
            dwEnable = 0;

            byte[] bin_dwEnrollNumber = new byte[4];
            byte[] bin_dwEMachineNumber = new byte[4];
            byte[] bin_dwBackupNumber = new byte[4];
            byte[] bin_dwMachinePrivilege = new byte[4];
            byte[] bin_dwEnable = new byte[4];

            GCHandle gh_dwEnrollNumber = GCHandle.Alloc(bin_dwEnrollNumber, GCHandleType.Pinned);
            GCHandle gh_dwEMachineNumber = GCHandle.Alloc(bin_dwEMachineNumber, GCHandleType.Pinned);
            GCHandle gh_dwBackupNumber = GCHandle.Alloc(bin_dwBackupNumber, GCHandleType.Pinned);
            GCHandle gh_dwMachinePrivilege = GCHandle.Alloc(bin_dwMachinePrivilege, GCHandleType.Pinned);
            GCHandle gh_dwEnable = GCHandle.Alloc(bin_dwEnable, GCHandleType.Pinned);

            try
            {
                IntPtr addr_dwEnrollNumber                = gh_dwEnrollNumber           .AddrOfPinnedObject();
                IntPtr addr_dwEMachineNumber              = gh_dwEMachineNumber         .AddrOfPinnedObject();
                IntPtr addr_dwBackupNumber                = gh_dwBackupNumber           .AddrOfPinnedObject();
                IntPtr addr_dwMachinePrivilege            = gh_dwMachinePrivilege       .AddrOfPinnedObject();
                IntPtr addr_dwEnable                      = gh_dwEnable                 .AddrOfPinnedObject();

                byte ret = _GetAllUserID(dwMachineNumber,
                                        addr_dwEnrollNumber       ,
                                        addr_dwEMachineNumber     ,
                                        addr_dwBackupNumber       ,
                                        addr_dwMachinePrivilege   ,
                                        addr_dwEnable);

                dwEnrollNumber      = new BinaryReader(new MemoryStream(bin_dwEnrollNumber     )).ReadInt32();
                dwEMachineNumber    = new BinaryReader(new MemoryStream(bin_dwEMachineNumber   )).ReadInt32();
                dwBackupNumber      = new BinaryReader(new MemoryStream(bin_dwBackupNumber     )).ReadInt32();
                dwMachinePrivilege  = new BinaryReader(new MemoryStream(bin_dwMachinePrivilege )).ReadInt32();
                dwEnable            = new BinaryReader(new MemoryStream(bin_dwEnable           )).ReadInt32();

                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                gh_dwEnrollNumber.Free();
                gh_dwEMachineNumber.Free();
                gh_dwBackupNumber.Free();
                gh_dwMachinePrivilege.Free();
                gh_dwEnable.Free();
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _GetSerialNumber(Int32 dwMachineNumber, ref IntPtr lpszSerialNumber);

        public static bool GetSerialNumber(Int32 dwMachineNumber, out string lpszSerialNumber)
        {
            lpszSerialNumber = "";
            IntPtr string_in = Marshal.StringToBSTR(lpszSerialNumber);
            try
            {
                byte ret = _GetSerialNumber(dwMachineNumber, ref string_in);
                lpszSerialNumber = Marshal.PtrToStringBSTR(string_in);
                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Marshal.FreeBSTR(string_in);
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern Int32 _GetBackupNumber(Int32 dwMachineNumber);
        public static Int32 GetBackupNumber(Int32 dwMachineNumber)
        {
            return _GetBackupNumber(dwMachineNumber);
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _GetProductCode(Int32 dwMachineNumber, ref IntPtr lpszProductCode);

        public static bool GetProductCode(Int32 dwMachineNumber, out string lpszProductCode)
        {
            lpszProductCode = "";
            IntPtr addr_product_code = Marshal.StringToBSTR(lpszProductCode);

            try
            {
                byte ret = _GetProductCode(dwMachineNumber, ref addr_product_code);
                lpszProductCode = Marshal.PtrToStringBSTR(addr_product_code);
                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Marshal.FreeBSTR(addr_product_code);
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _ClearKeeperData(Int32 dwMachineNumber);

        public static bool ClearKeeperData(Int32 dwMachineNumber)
        {
            return _ClearKeeperData(dwMachineNumber) > 0;
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _EmptyEnrollData(Int32 dwMachineNumber);

        public static bool EmptyEnrollData(Int32 dwMachineNumber)
        {
            return _EmptyEnrollData(dwMachineNumber) > 0;
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _EmptyGeneralLogData(Int32 dwMachineNumber);

        public static bool EmptyGeneralLogData(Int32 dwMachineNumber)
        {
            return _EmptyGeneralLogData(dwMachineNumber) > 0;
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _EmptySuperLogData(Int32 dwMachineNumber);

        public static bool EmptySuperLogData(Int32 dwMachineNumber)
        {
            return _EmptySuperLogData(dwMachineNumber) > 0;
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _GetUserName(Int32 DeviceKind, Int32 dwMachineNumber, Int32 dwEnrollNumber, Int32 dwEMachineNumber, out IntPtr lpszUserName);

        public static bool GetUserName(Int32 DeviceKind, Int32 dwMachineNumber, Int32 dwEnrollNumber, Int32 dwEMachineNumber, out string lpszUserName)
        {
            byte[] user_name = new byte[256];
            IntPtr userName;
            lpszUserName = "";

            try
            {
                byte ret = _GetUserName(DeviceKind, dwMachineNumber, dwEnrollNumber, dwEMachineNumber, out userName);
                lpszUserName = Marshal.PtrToStringUni(userName);

                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _GetUserName1(Int32 dwMachineNumber, Int32 dwEnrollNumber, out IntPtr lpszUserName);

        public static bool GetUserName1(Int32 dwMachineNumber, Int32 dwEnrollNumber, out string lpszUserName)
        {
            lpszUserName = "";
            IntPtr userName;
            try
            {
                byte ret = _GetUserName1(dwMachineNumber, dwEnrollNumber, out userName);
                lpszUserName = Marshal.PtrToStringUni(userName);

                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _SetUserName(Int32 DeviceKind,Int32 dwMachineNumber,Int32 dwEnrollNumber,Int32 dwEMachineNumber, ref IntPtr lpszUserName);

        public static bool SetUserName(Int32 DeviceKind, Int32 dwMachineNumber, Int32 dwEnrollNumber, Int32 dwEMachineNumber, string lpszUserName)
        {
            if (lpszUserName == null)
                return false;

            IntPtr addr_user_name = Marshal.StringToHGlobalUni(lpszUserName);

            try
            {
                return _SetUserName(DeviceKind, dwMachineNumber, dwEnrollNumber, dwEMachineNumber, ref addr_user_name) > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _SetUserName1(Int32 dwMachineNumber, Int32 dwEnrollNumber, ref IntPtr lpszUserName);

        public static bool SetUserName1(Int32 dwMachineNumber, Int32 dwEnrollNumber, string lpszUserName)
        {
            if (lpszUserName == null)
                return false;

            IntPtr addr_user_name = Marshal.StringToHGlobalUni(lpszUserName);

            try
            {
                return _SetUserName1(dwMachineNumber, dwEnrollNumber, ref addr_user_name) > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _GetCompanyName(Int32 dwMachineNumber, ref IntPtr dwCompanyName);

        public static bool GetCompanyName(Int32 dwMachineNumber, out string dwCompanyName)
        {
            byte[] company_name = new byte[256];
            GCHandle gh_company_name = GCHandle.Alloc(company_name, GCHandleType.Pinned);
            IntPtr addr_company_name = gh_company_name.AddrOfPinnedObject();
            dwCompanyName = "";

            try
            {
                byte ret = _GetCompanyName(dwMachineNumber, ref addr_company_name);
                int index = Array.FindIndex(company_name, c => c == 0);
                dwCompanyName = Encoding.Unicode.GetString(company_name, 0, index);

                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                gh_company_name.Free();
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _GetCompanyName1(Int32 dwMachineNumber, ref IntPtr dwCompanyName);

        public static bool GetCompanyName1(Int32 dwMachineNumber, out string dwCompanyName)
        {
            byte[] company_name = new byte[256];
            GCHandle gh_company_name = GCHandle.Alloc(company_name, GCHandleType.Pinned);
            IntPtr addr_company_name = gh_company_name.AddrOfPinnedObject();
            dwCompanyName = "";

            try
            {
                byte ret = _GetCompanyName1(dwMachineNumber, ref addr_company_name);
                int index = Array.FindIndex(company_name, c => c == 0);
                dwCompanyName = Encoding.Unicode.GetString(company_name, 0, index);

                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                gh_company_name.Free();
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _SetCompanyName(Int32 dwMachineNumber, Int32 vKind, ref IntPtr CompanyName);

        public static bool SetCompanyName(Int32 dwMachineNumber, Int32 vKind, string CompanyName)
        {
            if (CompanyName == null)
                return false;

            IntPtr addr_name = Marshal.StringToHGlobalUni(CompanyName);

            try
            {
                return _SetCompanyName(dwMachineNumber, vKind, ref addr_name) > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _SetCompanyName1(Int32 dwMachineNumber, Int32 vKind, ref IntPtr dwCompanyName);

        public static bool SetCompanyName1(Int32 dwMachineNumber, Int32 vKind, string CompanyName)
        {
            if (CompanyName == null)
                return false;

            byte[] temp_name = Encoding.Unicode.GetBytes(CompanyName);
            byte[] name = new byte[temp_name.Length + 1];
            Array.Copy(temp_name, name, temp_name.Length);

            GCHandle gh_name = GCHandle.Alloc(name, GCHandleType.Pinned);
            IntPtr addr_name = gh_name.AddrOfPinnedObject();

            try
            {
                return _SetCompanyName1(dwMachineNumber, vKind, ref addr_name) > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                gh_name.Free();
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _GetDoorStatus(Int32 dwMachineNumber, IntPtr dwValue);

        public static bool GetDoorStatus(Int32 dwMachineNumber, out Int32 dwValue)
        {
            dwValue = 0;

            byte[] bin_dwValue = new byte[4];

            GCHandle gh_dwValue = GCHandle.Alloc(bin_dwValue, GCHandleType.Pinned);
            try
            {
                IntPtr addr_dwValue = gh_dwValue.AddrOfPinnedObject();

                byte ret = _GetDoorStatus(dwMachineNumber, addr_dwValue);

                dwValue = new BinaryReader(new MemoryStream(bin_dwValue)).ReadInt32();

                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                gh_dwValue.Free();
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _SetDoorStatus(Int32 dwMachineNumber, Int32 dwValue);
        public static bool SetDoorStatus(Int32 dwMachineNumber, Int32 dwValue)
        {
            try
            {
                return _SetDoorStatus(dwMachineNumber, dwValue) > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _GetBellTime(Int32 dwMachineNumber, IntPtr dwValue, ref IntPtr dwBellInfo);

        public static bool GetBellTime(Int32 dwMachineNumber, out Int32 dwValue, IntPtr dwBellInfo)
        {
            dwValue = 0;

            byte[] bin_dwValue = new byte[4];

            GCHandle gh_dwValue = GCHandle.Alloc(bin_dwValue, GCHandleType.Pinned);

            IntPtr addr_dwValue = gh_dwValue.AddrOfPinnedObject();

            try
            {
                byte ret = _GetBellTime(dwMachineNumber, addr_dwValue, ref dwBellInfo);

                dwValue = new BinaryReader(new MemoryStream(bin_dwValue)).ReadInt32();

                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                gh_dwValue.Free();
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _SetBellTime(Int32 dwMachineNumber, Int32 dwValue, ref IntPtr dwBellInfo);

        public static bool SetBellTime(Int32 dwMachineNumber, Int32 dwValue, IntPtr dwBellInfo)
        {
            try
            {
                return _SetBellTime(dwMachineNumber, dwValue, ref dwBellInfo) > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _ConnectSerial(Int32 dwMachineNumber, Int32 dwCommPort, Int32 dwBaudRate);
        public static bool ConnectSerial(Int32 dwMachineNumber, Int32 dwCommPort, Int32 dwBaudRate)
        {
            try
            {
                return _ConnectSerial(dwMachineNumber, dwCommPort, dwBaudRate) > 0;
            }
            catch (System.Exception)
            {
                return false;
            }
        }
        #endregion

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _ConnectTcpip(Int32 dwMachineNumber, ref IntPtr lpszIPAddress, Int32 dwPortNumber, Int32 dwPassWord);

        public static bool ConnectTcpip(Int32 dwMachineNumber, string lpszIPAddress, Int32 dwPortNumber, Int32 dwPassWord)
        {
            if (lpszIPAddress == null)
                return false;

            IntPtr string_in = Marshal.StringToBSTR(lpszIPAddress);
            try
            {
                byte ret = _ConnectTcpip(dwMachineNumber, ref string_in, dwPortNumber, dwPassWord);
                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Marshal.FreeBSTR(string_in);
            }
        }

        #region Not Tested Part 3
        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern Int32 _ConnectP2p(ref IntPtr lpszMachineID, ref IntPtr lpszServerIPAddress, Int32 dwServerPortNumber, Int32 dwPassWord, IntPtr pnError);

        public static Int32 ConnectP2p(string lpszMachineID, string lpszServerIPAddress, Int32 dwServerPortNumber, Int32 dwPassWord, out Int32 pnError)
        {
            pnError = 0;

            if (lpszServerIPAddress == null || lpszMachineID == null)
                return 0;

            IntPtr str_server_ip = Marshal.StringToHGlobalUni(lpszServerIPAddress);
            IntPtr str_machine_id = Marshal.StringToHGlobalUni(lpszMachineID);

            byte[] error = new byte[4];
            GCHandle gh_error = GCHandle.Alloc(error, GCHandleType.Pinned);
            IntPtr ptr_error = gh_error.AddrOfPinnedObject();

            try
            {
                Int32 ret = _ConnectP2p(ref str_machine_id, ref str_server_ip, dwServerPortNumber, dwPassWord, ptr_error);

                pnError = new BinaryReader(new MemoryStream(error)).ReadInt32();

                return ret;
            }
            catch (Exception)
            {
                return 0;
            }
            finally
            {
                Marshal.FreeHGlobal(str_server_ip);
                Marshal.FreeHGlobal(str_machine_id);
                gh_error.Free();
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _PrepareP2p(ref IntPtr lpszMachineID, ref IntPtr lpszServerIPAddress, Int32 dwServerPortNumber,
            IntPtr pnStartYear, IntPtr pnStartMonth, IntPtr pnStartDay, IntPtr pnEndYear, IntPtr pnEndMonth, IntPtr pnEndDay, IntPtr pnError);

        public static bool _PrepareP2p(string lpszMachineID, string lpszServerIPAddress, Int32 dwServerPortNumber,
            out Int32 pnStartYear, out Int32 pnStartMonth, out Int32 pnStartDay, out Int32 pnEndYear, out Int32 pnEndMonth, out Int32 pnEndDay, out Int32 pnError)
        {
            pnStartYear = 0;
            pnStartMonth = 0;
            pnStartDay = 0;
            pnEndYear = 0;
            pnEndMonth = 0;
            pnEndDay = 0;
            pnError = 0;

            if (lpszServerIPAddress == null || lpszMachineID == null)
                return false;


            IntPtr str_server_ip = Marshal.StringToHGlobalUni(lpszServerIPAddress);
            IntPtr str_machine_id = Marshal.StringToHGlobalUni(lpszMachineID);

            byte[] bin_StartYear = new byte[4];
            byte[] bin_StartMonth = new byte[4];
            byte[] bin_StartDay = new byte[4];
            byte[] bin_EndYear = new byte[4];
            byte[] bin_EndMonth = new byte[4];
            byte[] bin_EndDay = new byte[4];
            byte[] bin_Error = new byte[4];

            GCHandle gh_StartYear             = GCHandle.Alloc(bin_StartYear         , GCHandleType.Pinned);
            GCHandle gh_StartMonth            = GCHandle.Alloc(bin_StartMonth        , GCHandleType.Pinned);
            GCHandle gh_StartDay              = GCHandle.Alloc(bin_StartDay          , GCHandleType.Pinned);
            GCHandle gh_EndYear               = GCHandle.Alloc(bin_EndYear           , GCHandleType.Pinned);
            GCHandle gh_EndMonth              = GCHandle.Alloc(bin_EndMonth          , GCHandleType.Pinned);
            GCHandle gh_EndDay                = GCHandle.Alloc(bin_EndDay            , GCHandleType.Pinned);
            GCHandle gh_Error                 = GCHandle.Alloc(bin_Error             , GCHandleType.Pinned);

            IntPtr ptr_StartYear = gh_StartYear.AddrOfPinnedObject();
            IntPtr ptr_StartMonth = gh_StartMonth.AddrOfPinnedObject();
            IntPtr ptr_StartDay = gh_StartDay.AddrOfPinnedObject();
            IntPtr ptr_EndYear = gh_EndYear.AddrOfPinnedObject();
            IntPtr ptr_EndMonth = gh_EndMonth.AddrOfPinnedObject();
            IntPtr ptr_EndDay = gh_EndDay.AddrOfPinnedObject();
            IntPtr ptr_Error = gh_Error.AddrOfPinnedObject();

            try
            {
                byte ret = _PrepareP2p(ref str_machine_id, ref str_server_ip, dwServerPortNumber,
                                            ptr_StartYear,
                                            ptr_StartMonth,
                                            ptr_StartDay,
                                            ptr_EndYear,
                                            ptr_EndMonth,
                                            ptr_EndDay,
                                            ptr_Error);

                pnStartYear = new BinaryReader(new MemoryStream(bin_StartYear)).ReadInt32();
                pnStartMonth = new BinaryReader(new MemoryStream(bin_StartMonth)).ReadInt32();
                pnStartDay = new BinaryReader(new MemoryStream(bin_StartDay)).ReadInt32();
                pnEndYear = new BinaryReader(new MemoryStream(bin_EndYear)).ReadInt32();
                pnEndMonth = new BinaryReader(new MemoryStream(bin_EndMonth)).ReadInt32();
                pnEndDay = new BinaryReader(new MemoryStream(bin_EndDay)).ReadInt32();
                pnError = new BinaryReader(new MemoryStream(bin_Error)).ReadInt32();

                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Marshal.FreeHGlobal(str_server_ip);
                Marshal.FreeHGlobal(str_machine_id);

                gh_StartYear.Free();
                gh_StartMonth.Free();
                gh_StartDay.Free();
                gh_EndYear.Free();
                gh_EndMonth.Free();
                gh_EndDay.Free();
                gh_Error.Free();
            }
        }
        #endregion

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern void _Disconnect(Int32 dwMachineNumber);
        public static void Disconnect(Int32 dwMachineNumber)
        {
            _Disconnect(dwMachineNumber);
        }

        #region Not Tested Part 4
        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _GetLastError(Int32 dwMachineNumber, IntPtr dwErrorCode);

        public static bool GetLastError(Int32 dwMachineNumber, out Int32 dwErrorCode)
        {
            dwErrorCode = 0;

            byte[] error_code = new byte[4];
            GCHandle gh = GCHandle.Alloc(error_code, GCHandleType.Pinned);
            IntPtr addr = gh.AddrOfPinnedObject();
            try
            {
                byte ret = _GetLastError(dwMachineNumber, addr);
                dwErrorCode = new BinaryReader(new MemoryStream(error_code)).ReadInt32();

                return ret > 0;
            }
            catch (System.Exception)
            {
                return false;
            }
            finally
            {
                gh.Free();
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _GetDeviceLongInfo(Int32 dwMachineNumber, Int32 dwInfo, ref IntPtr dwValue);

        public static bool GetDeviceLongInfo(Int32 dwMachineNumber, Int32 dwInfo, IntPtr dwValue)
        {
            try
            {
                return _GetDeviceLongInfo(dwMachineNumber, dwInfo, ref dwValue) > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _SetDeviceLongInfo(Int32 dwMachineNumber, Int32 dwInfo, ref IntPtr dwValue);

        public static bool SetDeviceLongInfo(Int32 dwMachineNumber, Int32 dwInfo, IntPtr dwValue)
        {
            try
            {
                return _SetDeviceLongInfo(dwMachineNumber, dwInfo, ref dwValue) > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _ModifyDuressFP(Int32 dwMachineNumber, Int32 dwEnrollNumber, Int32 dwBackupNumber, Int32 dwDuressSetting);
        public static bool ModifyDuressFP(Int32 dwMachineNumber, Int32 dwEnrollNumber, Int32 dwBackupNumber, Int32 dwDuressSetting)
        {
            try
            {
                return _ModifyDuressFP(dwMachineNumber, dwEnrollNumber, dwBackupNumber, dwDuressSetting) > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _GetMachineIP( IntPtr lpszProduct, Int32 dwMachineNumber, IntPtr lpszIPBuf );

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _GetDepartName(Int32 dwMachineNumber, Int32 dwDepartNumber, Int32 dwDepartOrDaigong, ref IntPtr lpszDepartName);
        public static bool GetDepartName(Int32 dwMachineNumber, Int32 dwDepartNumber, Int32 dwDepartOrDaigong, out string lpszDepartName)
        {
            lpszDepartName = "";
            IntPtr addr_depart_name = Marshal.StringToBSTR(lpszDepartName);

            try
            {
                byte ret = _GetDepartName(dwMachineNumber, dwDepartNumber, dwDepartOrDaigong, ref addr_depart_name);
                lpszDepartName = Marshal.PtrToStringBSTR(addr_depart_name);
                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Marshal.FreeBSTR(addr_depart_name);
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _SetDepartName(Int32 dwMachineNumber, Int32 dwDepartNumber, Int32 dwDepartOrDaigong, ref IntPtr lpszDepartName);

        public static bool SetDepartName(Int32 dwMachineNumber, Int32 dwDepartNumber, Int32 dwDepartOrDaigong, string lpszDepartName)
        {
            if (lpszDepartName == null)
                return false;

            byte[] temp_name = Encoding.Unicode.GetBytes(lpszDepartName);
            byte[] name = new byte[temp_name.Length + 1];
            Array.Copy(temp_name, name, temp_name.Length);

            GCHandle gh_name = GCHandle.Alloc(name, GCHandleType.Pinned);
            IntPtr addr_name = gh_name.AddrOfPinnedObject();

            try
            {
                return _SetDepartName(dwMachineNumber, dwDepartNumber, dwDepartOrDaigong, ref addr_name) > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                gh_name.Free();
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _GeneralOperationXML(Int32 dwMachineNumber, ref IntPtr lpszReqNResXML);

        public static bool GeneralOperationXML(Int32 dwMachineNumber, ref string lpszReqNResXML)
        {
            IntPtr string_in = Marshal.StringToBSTR(lpszReqNResXML);

            try
            {
                byte ret = _GeneralOperationXML(dwMachineNumber, ref string_in);
                lpszReqNResXML = Marshal.PtrToStringBSTR(string_in);
                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Marshal.FreeBSTR(string_in);
            }
        }

        //xml helper interfaces
        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern Int32 _XML_ParseInt(ref IntPtr lpszXML, IntPtr lpszTagname);
        public static Int32 XML_ParseInt(ref string lpszXML, string lpszTagname) // TODO KGH
        {
            IntPtr string_in = Marshal.StringToBSTR(lpszXML);
            IntPtr tag = Marshal.StringToBSTR(lpszTagname);

            try
            {
                return  _XML_ParseInt(ref string_in, tag);
            }
            catch (Exception)
            {
                return -1;
            }
            finally
            {
                Marshal.FreeBSTR(string_in);
                Marshal.FreeBSTR(tag);
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern Int32 _XML_ParseLong(ref IntPtr lpszXML, IntPtr lpszTagname);
        public static Int32 XML_ParseLong(ref string lpszXML, string lpszTagname) // TODO KGH
        {
            IntPtr string_in = Marshal.StringToBSTR(lpszXML);
            IntPtr tag = Marshal.StringToBSTR(lpszTagname);

            try
            {
                return _XML_ParseLong(ref string_in, tag);
            }
            catch (Exception)
            {
                return -1;
            }
            finally
            {
                Marshal.FreeBSTR(string_in);
                Marshal.FreeBSTR(tag);
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _XML_ParseBoolean(ref IntPtr lpszXML, IntPtr lpszTagname);
        public static bool _XML_ParseBoolean(ref string lpszXML, string lpszTagname) // TODO KGH
        {
            IntPtr string_in = Marshal.StringToBSTR(lpszXML);
            IntPtr tag = Marshal.StringToBSTR(lpszTagname);

            try
            {
                return _XML_ParseBoolean(ref string_in, tag) > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Marshal.FreeBSTR(string_in);
                Marshal.FreeBSTR(tag);
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _XML_ParseString(ref IntPtr lpszXML, IntPtr lpszTagname, ref IntPtr lpszValue);
        public static bool XML_ParseString(ref string lpszXML, string lpszTagname, out string lpszValue) // TODO KGH
        {
            IntPtr string_in = Marshal.StringToBSTR(lpszXML);
            IntPtr tag = Marshal.StringToBSTR(lpszTagname);
            lpszValue = "";
            IntPtr val = Marshal.StringToBSTR(lpszValue);

            try
            {
                byte ret = _XML_ParseString(ref string_in, tag, ref val);
                lpszValue = Marshal.PtrToStringBSTR(val);
                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Marshal.FreeBSTR(string_in);
                Marshal.FreeBSTR(tag);
                Marshal.FreeBSTR(val);
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _XML_ParseBinaryByte(ref IntPtr lpszXML, IntPtr lpszTagname, ref IntPtr pData, Int32 dwLen);
        public static bool XML_ParseBinaryByte(ref string lpszXML, string lpszTagname, IntPtr pData, Int32 dwLen) // TODO KGH
        {
            IntPtr string_in = Marshal.StringToBSTR(lpszXML);
            IntPtr tag = Marshal.StringToBSTR(lpszTagname);

            try
            {
                return _XML_ParseBinaryByte(ref string_in, tag, ref pData, dwLen) > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Marshal.FreeBSTR(string_in);
                Marshal.FreeBSTR(tag);
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _XML_ParseBinaryWord(ref IntPtr lpszXML, IntPtr lpszTagname, ref IntPtr pData, Int32 dwLen);
        public static bool XML_ParseBinaryWord(ref string lpszXML, string lpszTagname, IntPtr pData, Int32 dwLen) // TODO KGH
        {
            IntPtr string_in = Marshal.StringToBSTR(lpszXML);
            IntPtr tag = Marshal.StringToBSTR(lpszTagname);

            try
            {
                return _XML_ParseBinaryWord(ref string_in, tag, ref pData, dwLen) > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Marshal.FreeBSTR(string_in);
                Marshal.FreeBSTR(tag);
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _XML_ParseBinaryLong(ref IntPtr lpszXML, IntPtr lpszTagname, ref IntPtr pData, Int32 dwLen);
        public static bool XML_ParseBinaryLong(ref string lpszXML, string lpszTagname, IntPtr pData, Int32 dwLen) // TODO KGH
        {
            IntPtr string_in = Marshal.StringToBSTR(lpszXML);
            IntPtr tag = Marshal.StringToBSTR(lpszTagname);

            try
            {
                return _XML_ParseBinaryLong(ref string_in, tag, ref pData, dwLen) > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Marshal.FreeBSTR(string_in);
                Marshal.FreeBSTR(tag);
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _XML_ParseBinaryUnicode(ref IntPtr lpszXML, IntPtr lpszTagname, ref IntPtr pData, Int32 dwLen);
        public static bool XML_ParseBinaryUnicode(ref string lpszXML, string lpszTagname, out string pData, Int32 dwLen) // TODO KGH
        {
            IntPtr string_in = Marshal.StringToBSTR(lpszXML);
            IntPtr tag = Marshal.StringToBSTR(lpszTagname);
            pData = "";
            IntPtr val = Marshal.StringToBSTR(pData);
            try
            {
                byte ret = _XML_ParseBinaryUnicode(ref string_in, tag, ref val, dwLen);
                pData = Marshal.PtrToStringBSTR(val);
                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Marshal.FreeBSTR(string_in);
                Marshal.FreeBSTR(tag);
                Marshal.FreeBSTR(val);
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _XML_ParseMultiUnicode(ref IntPtr lpszXML, IntPtr lpszTagname, ref IntPtr pData, Int32 dwLen);
        public static bool XML_ParseMultiUnicode(ref string lpszXML, string lpszTagname, out string pData, Int32 dwLen) // TODO KGH
        {
            IntPtr string_in = Marshal.StringToBSTR(lpszXML);
            IntPtr tag = Marshal.StringToBSTR(lpszTagname);
            pData = "";
            IntPtr val = Marshal.StringToBSTR(pData);
            try
            {
                byte ret = _XML_ParseMultiUnicode(ref string_in, tag, ref val, dwLen);
                pData = Marshal.PtrToStringBSTR(val);
                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Marshal.FreeBSTR(string_in);
                Marshal.FreeBSTR(tag);
                Marshal.FreeBSTR(val);
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _XML_AddInt(ref IntPtr lpszXML, IntPtr lpszTagname, int nValue);
        public static bool XML_AddInt(ref string lpszXML, string lpszTagname, int nValue) // TODO KGH
        {
            IntPtr string_in = Marshal.StringToBSTR(lpszXML);
            IntPtr tag = Marshal.StringToBSTR(lpszTagname);
            try
            {
                byte ret = _XML_AddInt(ref string_in, tag, nValue);
                lpszXML = Marshal.PtrToStringBSTR(string_in);
                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Marshal.FreeBSTR(string_in);
                Marshal.FreeBSTR(tag);
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _XML_AddLong(ref IntPtr lpszXML, IntPtr lpszTagname, Int32 dwValue);
        public static bool XML_AddLong(ref string lpszXML, string lpszTagname, Int32 dwValue) // TODO KGH
        {
            IntPtr string_in = Marshal.StringToBSTR(lpszXML);
            IntPtr tag = Marshal.StringToBSTR(lpszTagname);
            try
            {
                byte ret = _XML_AddLong(ref string_in, tag, dwValue);
                lpszXML = Marshal.PtrToStringBSTR(string_in);
                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Marshal.FreeBSTR(string_in);
                Marshal.FreeBSTR(tag);
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _XML_AddBoolean(ref IntPtr lpszXML, IntPtr lpszTagname, byte bValue);
        public static bool XML_AddBoolean(ref string lpszXML, string lpszTagname, bool bValue) // TODO KGH
        {
            IntPtr string_in = Marshal.StringToBSTR(lpszXML);
            IntPtr tag = Marshal.StringToBSTR(lpszTagname);
            try
            {
                byte ret = _XML_AddBoolean(ref string_in, tag, bValue ? (byte)1 : (byte)0);
                lpszXML = Marshal.PtrToStringBSTR(string_in);
                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Marshal.FreeBSTR(string_in);
                Marshal.FreeBSTR(tag);
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _XML_AddString(ref IntPtr lpszXML, IntPtr lpszTagname, IntPtr lpszValue);
        public static bool XML_AddString(ref string lpszXML, string lpszTagname, string lpszValue) // TODO KGH
        {
            IntPtr string_in = Marshal.StringToBSTR(lpszXML);
            IntPtr tag = Marshal.StringToBSTR(lpszTagname);
            IntPtr value = Marshal.StringToHGlobalUni(lpszValue);
            try
            {
                byte ret = _XML_AddString(ref string_in, tag, value);
                lpszXML = Marshal.PtrToStringBSTR(string_in);
                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Marshal.FreeBSTR(string_in);
                Marshal.FreeBSTR(tag);
                Marshal.FreeHGlobal(value);
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _XML_AddBinaryByte(ref IntPtr lpszXML, IntPtr lpszTagname, IntPtr dwData, Int32 dwLen);
        public static bool XML_AddBinaryByte(ref string lpszXML, string lpszTagname, IntPtr dwData, Int32 dwLen) // TODO KGH
        {
            IntPtr string_in = Marshal.StringToBSTR(lpszXML);
            IntPtr tag = Marshal.StringToBSTR(lpszTagname);
            try
            {
                byte ret = _XML_AddBinaryByte(ref string_in, tag, dwData, dwLen);
                lpszXML = Marshal.PtrToStringBSTR(string_in);
                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Marshal.FreeBSTR(string_in);
                Marshal.FreeBSTR(tag);
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _XML_AddBinaryWord(ref IntPtr lpszXML, IntPtr lpszTagname, IntPtr dwData, Int32 dwLen);
        public static bool XML_AddBinaryWord(ref string lpszXML, string lpszTagname, IntPtr dwData, Int32 dwLen) // TODO KGH
        {
            IntPtr string_in = Marshal.StringToBSTR(lpszXML);
            IntPtr tag = Marshal.StringToBSTR(lpszTagname);
            try
            {
                byte ret = _XML_AddBinaryWord(ref string_in, tag, dwData, dwLen);
                lpszXML = Marshal.PtrToStringBSTR(string_in);
                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Marshal.FreeBSTR(string_in);
                Marshal.FreeBSTR(tag);
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _XML_AddBinaryLong(ref IntPtr lpszXML, IntPtr lpszTagname, IntPtr dwData, Int32 dwLen);
        public static bool XML_AddBinaryLong(ref string lpszXML, string lpszTagname, IntPtr dwData, Int32 dwLen) // TODO KGH
        {
            IntPtr string_in = Marshal.StringToBSTR(lpszXML);
            IntPtr tag = Marshal.StringToBSTR(lpszTagname);
            try
            {
                byte ret = _XML_AddBinaryLong(ref string_in, tag, dwData, dwLen);
                lpszXML = Marshal.PtrToStringBSTR(string_in);
                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Marshal.FreeBSTR(string_in);
                Marshal.FreeBSTR(tag);
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _XML_AddBinaryUnicode(ref IntPtr lpszXML, IntPtr lpszTagname, IntPtr lpszData);
        public static bool XML_AddBinaryUnicode(ref string lpszXML, string lpszTagname, string lpszData) // TODO KGH
        {
            IntPtr string_in = Marshal.StringToBSTR(lpszXML);
            IntPtr tag = Marshal.StringToBSTR(lpszTagname);
            IntPtr val = Marshal.StringToHGlobalUni(lpszData);
            try
            {
                byte ret = _XML_AddBinaryUnicode(ref string_in, tag, val);
                lpszXML = Marshal.PtrToStringBSTR(string_in);
                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Marshal.FreeBSTR(string_in);
                Marshal.FreeBSTR(tag);
                Marshal.FreeHGlobal(val);
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _XML_AddBinaryGlyph(ref IntPtr lpszXML, IntPtr lpszMessage, Int32 width, Int32 height, IntPtr lpszFontDescriptor);
        public static bool XML_AddBinaryGlyph(ref string lpszXML, string lpszMessage, Int32 width, Int32 height, string lpszFontDescriptor) // TODO KGH
        {
            IntPtr string_in = Marshal.StringToBSTR(lpszXML);
            IntPtr msg = Marshal.StringToBSTR(lpszMessage);
            IntPtr font = Marshal.StringToHGlobalUni(lpszFontDescriptor);
            try
            {
                byte ret = _XML_AddBinaryGlyph(ref string_in, msg, width, height, font);
                lpszXML = Marshal.PtrToStringBSTR(string_in);
                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Marshal.FreeBSTR(string_in);
                Marshal.FreeBSTR(msg);
                Marshal.FreeHGlobal(font);
            }
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _XML_AddBinaryNameGlyph(Int32 dwMachineNumber, ref IntPtr lpszXML, IntPtr lpszName);
        public static bool XML_AddBinaryNameGlyph(Int32 dwMachineNumber, ref string lpszXML, string lpszName) // TODO KGH
        {
            IntPtr string_in = Marshal.StringToBSTR(lpszXML);
            IntPtr name = Marshal.StringToBSTR(lpszName);
            try
            {
                byte ret = _XML_AddBinaryNameGlyph(dwMachineNumber, ref string_in, name);
                lpszXML = Marshal.PtrToStringBSTR(string_in);
                return ret > 0;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Marshal.FreeBSTR(string_in);
                Marshal.FreeBSTR(name);
            }
        }
        #endregion

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern void _DisconnectAll();
        public static void DisconnectAll()
        {
            _DisconnectAll();
        }

        public delegate byte TransceiveCallback(Int32 dwCurrent, Int32 dwTotal);
        static TransceiveCallback transceive_call_back = DisabledTransceiveCallback;

        static byte DisabledTransceiveCallback(Int32 dwCurrent, Int32 dwTotal)
        {
            return 1;
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern void _SetTranseiveCallback(TransceiveCallback callback);

        public static void _DisableTranseiveCallback()
        {
            try
            {
                _SetTranseiveCallback(transceive_call_back);
            }
            catch (Exception )
            {
                            	
            }
            finally
            {
            }
        }

        public delegate void EventCallback_Core(IntPtr context, IntPtr xml);

        static void OnEvent(IntPtr context, IntPtr xml)
        {
            if (event_callback == null)
                return;

            event_callback(Marshal.PtrToStringAnsi(xml));
        }

        private static EventCallback_Core core_event_callback = OnEvent;

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern byte _StartEventCapture(Int32 dwCommType, Int32 dwParam1, Int32 dwParam2, EventCallback_Core cb, IntPtr context);

        public delegate void EventCallback(string eventXML);
        private static EventCallback event_callback = null;
        public static bool StartEventCapture(Int32 dwCommType, Int32 dwParam1, Int32 dwParam2, EventCallback cb, IntPtr context)
        {
            event_callback = cb;
            return _StartEventCapture(dwCommType, dwParam1, dwParam2, core_event_callback, context) > 0;
        }

        [DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
        static extern void _StopEventCapture();
        public static void StopEventCapture()
        {
            _StopEventCapture();
        }

		[DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
		static extern bool _ReadGLogWithPos(Int32 dwMachineNumber, Int32 dwStartPos, Int32 dwEndPos);
		public static bool ReadGLogWithPos(int dwMachineNumber, int dwStartPos, int dwEndPos)
		{
			return _ReadGLogWithPos(dwMachineNumber, dwStartPos, dwEndPos);
		}

		[DllImport("SBXPCDLL.dll", CallingConvention = CallingConvention.Winapi)]
		static extern bool _ReadSLogWithPos(Int32 dwMachineNumber, Int32 dwStartPos, Int32 dwEndPos);
		public static bool ReadSLogWithPos(int dwMachineNumber, int dwStartPos, int dwEndPos)
		{
			return _ReadSLogWithPos(dwMachineNumber, dwStartPos, dwEndPos);
		}
    }
}
