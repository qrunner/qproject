using System;
using System.Runtime.InteropServices;

namespace Common.WinAPI
{
    public class UNC
    {
        #region WinAPI
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct USE_INFO_2
        {
            internal String ui2_local;
            internal String ui2_remote;
            internal String ui2_password;
            internal UInt32 ui2_status;
            internal UInt32 ui2_asg_type;
            internal UInt32 ui2_refcount;
            internal UInt32 ui2_usecount;
            internal String ui2_username;
            internal String ui2_domainname;
        }

        [DllImport("NetApi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern UInt32 NetUseAdd(
            String UncServerName,
            UInt32 Level,
            ref USE_INFO_2 Buf,
            out UInt32 ParmError);

        [DllImport("NetApi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern UInt32 NetUseDel(
            String UncServerName,
            String UseName,
            UInt32 ForceCond);
        #endregion

        /// <summary>
        /// Открытие доступа на папку по учетной записи
        /// </summary>
        /// <param name="path"></param>
        /// <param name="domain"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static uint NetUseAdd(string path, string domain, string user, string password)
        {
            USE_INFO_2 useinfo = new USE_INFO_2();
            useinfo.ui2_remote = path;
            useinfo.ui2_username = user;
            useinfo.ui2_domainname = domain;
            useinfo.ui2_password = password;
            useinfo.ui2_asg_type = 0;
            useinfo.ui2_usecount = 1;
            uint paramErrorIndex;
            uint code = NetUseAdd(null, 2, ref useinfo, out paramErrorIndex);
            return code;
        }

        /// <summary>
        /// Закрытие доступа к папке по учетной записи
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static uint NetUseDel(string path)
        {
            return NetUseDel(null, path, 2);
        }
    }
}
