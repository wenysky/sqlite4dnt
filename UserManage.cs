using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using Discuz.Data;
using Discuz.Config;
using Discuz.Common;
using Discuz.Entity;

namespace Discuz.Data.Sqlite
{
    public partial class DataProvider : IDataProvider
    {

        private static int _lastRemoveTimeout;

        public DataTable GetUsers(string idlist)
        {
            if (!Utils.IsNumericArray(idlist.Split(',')))
                return new DataTable();

            string sql = string.Format("SELECT [uid],[username] FROM [{0}users] WHERE [groupid] IN ({1})", BaseConfigs.GetTablePrefix, idlist);
            return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public DataTable GetUserGroupInfoByGroupid(int groupid)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@groupid",DbType.Int32, 4,groupid)
			};
            string sql = "SELECT TOP 1 * FROM  [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]=@groupid";
            return DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0];
        }

        public DataTable GetAdmingroupByAdmingid(int admingid)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@admingid",DbType.Int32, 4,admingid)
			};
            string sql = "SELECT TOP 1 * FROM  [" + BaseConfigs.GetTablePrefix + "admingroups] WHERE [admingid]=@admingid";
            return DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0];
        }

        public DataTable GetMedal()
        {
            string sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "medals]";
            return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public string GetMedalSql()
        {
            return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "medals]";
        }

        public DataTable GetExistMedalList()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [medalid],[image] FROM [" + BaseConfigs.GetTablePrefix + "medals] WHERE [image]<>''").Tables[0];
        }

        public void AddMedal(int medalid, string name, int available, string image)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@medalid", DbType.Int16,2, medalid),
				DbHelper.MakeInParam("@name", DbType.String,50, name),
                DbHelper.MakeInParam("@available", DbType.Int32, 4, available),
				DbHelper.MakeInParam("@image",DbType.AnsiString,30,image)
			};
            string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "medals] (medalid,name,available,image) Values (@medalid,@name,@available,@image)";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public void UpdateMedal(int medalid, string name, string image)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@medalid", DbType.Int16,2, medalid),
				DbHelper.MakeInParam("@name", DbType.String,50, name),
				DbHelper.MakeInParam("@image",DbType.AnsiString,30,image)
			};
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "medals] SET [name]=@name,[image]=@image  Where [medalid]=@medalid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public void SetAvailableForMedal(int available, string medailidlist)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@available", DbType.Int32, 4, available)
			};
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "medals] SET [available]=@available WHERE [medalid] IN(" + medailidlist + ")";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public void DeleteMedalById(string medailidlist)
        {
            string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "medals] WHERE [medalid] IN(" + medailidlist + ")";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql);
        }

        public int GetMaxMedalId()
        {
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT ISNULL(MAX(medalid), 0) FROM " + BaseConfigs.GetTablePrefix + "medals"), 0) + 1;
        }

        public string GetGroupInfo()
        {
            string sql = "SELECT [groupid], [grouptitle] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] ORDER BY [groupid]";
            return sql;
        }

        /// <summary>
        /// 获得到指定管理组信息
        /// </summary>
        /// <returns>管理组信息</returns>
        public DataTable GetAdminGroupList()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "admingroups]").Tables[0];
        }

        /// <summary>
        /// 设置管理组信息
        /// </summary>
        /// <param name="__admingroupsInfo">管理组信息</param>
        /// <returns>更改记录数</returns>
        public int SetAdminGroupInfo(AdminGroupInfo admingroupsInfo)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@admingid",DbType.Int16,2,admingroupsInfo.Admingid),
									   DbHelper.MakeInParam("@alloweditpost",DbType.Byte,1,admingroupsInfo.Alloweditpost),
									   DbHelper.MakeInParam("@alloweditpoll",DbType.Byte,1,admingroupsInfo.Alloweditpoll),
									   DbHelper.MakeInParam("@allowstickthread",DbType.Byte,1,admingroupsInfo.Allowstickthread),
									   DbHelper.MakeInParam("@allowmodpost",DbType.Byte,1,admingroupsInfo.Allowmodpost),
									   DbHelper.MakeInParam("@allowdelpost",DbType.Byte,1,admingroupsInfo.Allowdelpost),
									   DbHelper.MakeInParam("@allowmassprune",DbType.Byte,1,admingroupsInfo.Allowmassprune),
									   DbHelper.MakeInParam("@allowrefund",DbType.Byte,1,admingroupsInfo.Allowrefund),
									   DbHelper.MakeInParam("@allowcensorword",DbType.Byte,1,admingroupsInfo.Allowcensorword),
									   DbHelper.MakeInParam("@allowviewip",DbType.Byte,1,admingroupsInfo.Allowviewip),
									   DbHelper.MakeInParam("@allowbanip",DbType.Byte,1,admingroupsInfo.Allowbanip),
									   DbHelper.MakeInParam("@allowedituser",DbType.Byte,1,admingroupsInfo.Allowedituser),
									   DbHelper.MakeInParam("@allowmoduser",DbType.Byte,1,admingroupsInfo.Allowmoduser),
									   DbHelper.MakeInParam("@allowbanuser",DbType.Byte,1,admingroupsInfo.Allowbanuser),
									   DbHelper.MakeInParam("@allowpostannounce",DbType.Byte,1,admingroupsInfo.Allowpostannounce),
									   DbHelper.MakeInParam("@allowviewlog",DbType.Byte,1,admingroupsInfo.Allowviewlog),
									   DbHelper.MakeInParam("@disablepostctrl",DbType.Byte,1,admingroupsInfo.Disablepostctrl),
                                       DbHelper.MakeInParam("@allowviewrealname",DbType.Byte,1,admingroupsInfo.Allowviewrealname)
								   };
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "updateadmingroup", parms);
        }

        /// <summary>
        /// 创建一个新的管理组信息
        /// </summary>
        /// <param name="__admingroupsInfo">要添加的管理组信息</param>
        /// <returns>更改记录数</returns>
        public int CreateAdminGroupInfo(AdminGroupInfo admingroupsInfo)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@admingid",DbType.Int16,2,admingroupsInfo.Admingid),
									   DbHelper.MakeInParam("@alloweditpost",DbType.Byte,1,admingroupsInfo.Alloweditpost),
									   DbHelper.MakeInParam("@alloweditpoll",DbType.Byte,1,admingroupsInfo.Alloweditpoll),
									   DbHelper.MakeInParam("@allowstickthread",DbType.Byte,1,admingroupsInfo.Allowstickthread),
									   DbHelper.MakeInParam("@allowmodpost",DbType.Byte,1,admingroupsInfo.Allowmodpost),
									   DbHelper.MakeInParam("@allowdelpost",DbType.Byte,1,admingroupsInfo.Allowdelpost),
									   DbHelper.MakeInParam("@allowmassprune",DbType.Byte,1,admingroupsInfo.Allowmassprune),
									   DbHelper.MakeInParam("@allowrefund",DbType.Byte,1,admingroupsInfo.Allowrefund),
									   DbHelper.MakeInParam("@allowcensorword",DbType.Byte,1,admingroupsInfo.Allowcensorword),
									   DbHelper.MakeInParam("@allowviewip",DbType.Byte,1,admingroupsInfo.Allowviewip),
									   DbHelper.MakeInParam("@allowbanip",DbType.Byte,1,admingroupsInfo.Allowbanip),
									   DbHelper.MakeInParam("@allowedituser",DbType.Byte,1,admingroupsInfo.Allowedituser),
									   DbHelper.MakeInParam("@allowmoduser",DbType.Byte,1,admingroupsInfo.Allowmoduser),
									   DbHelper.MakeInParam("@allowbanuser",DbType.Byte,1,admingroupsInfo.Allowbanuser),
									   DbHelper.MakeInParam("@allowpostannounce",DbType.Byte,1,admingroupsInfo.Allowpostannounce),
									   DbHelper.MakeInParam("@allowviewlog",DbType.Byte,1,admingroupsInfo.Allowviewlog),
									   DbHelper.MakeInParam("@disablepostctrl",DbType.Byte,1,admingroupsInfo.Disablepostctrl),
                                       DbHelper.MakeInParam("@allowviewrealname",DbType.Byte,1,admingroupsInfo.Allowviewrealname)
								   };
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "createadmingroup", parms);
        }

        /// <summary>
        /// 删除指定的管理组信息
        /// </summary>
        /// <param name="admingid">管理组ID</param>
        /// <returns>更改记录数</returns>
        public int DeleteAdminGroupInfo(short admingid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@admingid",DbType.Int16,2,admingid),
								   };
            return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "admingroups] WHERE [admingid] = @admingid", parms);
        }

        public string GetAdminGroupInfoSql()
        {
            return "Select * From [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [radminid]>0 AND [radminid]<=3  Order By [groupid]";
        }

        public DataTable GetRaterangeByGroupid(int groupid)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@groupid",DbType.Int32, 4,groupid)
			};
            string sql = "SELECT TOP 1 [raterange] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]=@groupid";
            return DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0];
        }

        public void UpdateRaterangeByGroupid(string raterange, int groupid)
        {
            DbParameter[] parms = 
			{
                DbHelper.MakeInParam("@raterange",DbType.AnsiString, 500,raterange),
				DbHelper.MakeInParam("@groupid",DbType.Int32, 4,groupid)
			};
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "usergroups] SET [raterange]=@raterange WHERE [groupid]=@groupid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public string GetAudituserSql()
        {
            return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "users] Where [groupid]=8";
        }

        public DataSet GetAudituserUid()
        {
            string sql = "SELECT [uid] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [groupid]=8";
            return DbHelper.ExecuteDataset(CommandType.Text, sql);
        }

        public void ClearAuthstrByUidlist(string uidlist)
        {
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "userfields] SET [authstr]='' WHERE [uid] IN (" + uidlist + ")";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql);
        }

        public void ClearAllUserAuthstr()
        {
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "userfields] SET [authstr]='' WHERE [uid] IN (SELECT [uid] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [groupid]=8 )";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql);
        }

        public void DeleteUserByUidlist(string uidlist)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "userfields] WHERE [uid] IN(" + uidlist + ")");
            DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid] IN(" + uidlist + ")");
        }

        public void DeleteAuditUser()
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "userfields] WHERE [uid] IN (SELECT [uid] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [groupid]=8 )");
            DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [groupid]=8 ");
        }

        public DataTable GetAuditUserEmail()
        {
            string sql = "SELECT [username],[password],[email] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [groupid]=8";
            return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }
        public DataTable GetUserEmailByUidlist(string uidlist)
        {
            string sql = "SELECT [username],[password],[email] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid] IN(" + uidlist + ")";
            return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public string GetUserGroup()
        {
            string sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [radminid]= 0 And [groupid]>8 ORDER BY [groupid]";
            return sql;
        }

        public string GetUserGroupTitle()
        {
            return "SELECT [groupid],[grouptitle] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [radminid]= 0 And [groupid]>8 ORDER BY [groupid]";
        }

        public DataTable GetUserGroupWithOutGuestTitle()
        {
            return DbHelper.ExecuteDataset("SELECT [groupid],[grouptitle] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]<>7  ORDER BY [groupid] ASC").Tables[0];
        }

        public string GetAdminUserGroupTitle()
        {
            string sql = "SELECT [groupid],[grouptitle] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [radminid]> 0 AND [radminid]<=3  ORDER BY [groupid]";
            return sql;
        }

        public void CombinationUsergroupScore(int sourceusergroupid, int targetusergroupid)
        {
            DbParameter[] parms = 
			{
                DbHelper.MakeInParam("@sourceusergroupid",DbType.Int32, 4,sourceusergroupid),
				DbHelper.MakeInParam("@targetusergroupid",DbType.Int32, 4,targetusergroupid)
			};
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "usergroups] SET [creditshigher]=(SELECT [creditshigher] FROM "
                + "[" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]=@sourceusergroupid) WHERE [groupid]=@targetusergroupid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public void DeleteUserGroupInfo(int groupid)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@groupid",DbType.Int32, 4,groupid)
			};
            string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "usergroups] Where [groupid]=@groupid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public void DeleteAdminGroupInfo(int admingid)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@admingid",DbType.Int32, 4,admingid)
			};
            string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "admingroups] Where [admingid]=@admingid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public void ChangeUsergroup(int soureceusergroupid, int targetusergroupid)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@soureceusergroupid",DbType.Int32, 4,soureceusergroupid),
                DbHelper.MakeInParam("@targetusergroupid",DbType.Int32, 4,targetusergroupid)
			};
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [groupid]=@targetusergroupid WHERE [groupid]=@soureceusergroupid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }
        public DataTable GetAdmingid(int admingid)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@admingid",DbType.Int32, 4,admingid)
			};
            string sql = "SELECT [admingid]  FROM [" + BaseConfigs.GetTablePrefix + "admingroups] WHERE [admingid]=@admingid";
            return DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0];
        }

        public void ChangeUserAdminidByGroupid(int adminid, int groupid)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@adminid",DbType.Int32, 4,adminid),
                DbHelper.MakeInParam("@groupid",DbType.Int32, 4,groupid)
			};
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [adminid]=@adminid WHERE [groupid]=@groupid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public DataTable GetAvailableMedal()
        {
            string sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "medals] WHERE [available]=1";
            return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public bool IsExistMedalAwardRecord(int medalid, int userid)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@medalid", DbType.Int32,4, medalid),
				DbHelper.MakeInParam("@userid",DbType.Int32,4,userid)
			};
            string sql = "SELECT TOP 1 ID FROM [" + BaseConfigs.GetTablePrefix + "medalslog] WHERE [medals]=@medalid AND [uid]=@userid";
            if (DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0].Rows.Count != 0)
                return true;
            else
                return false;
        }

        public void AddMedalslog(int adminid, string adminname, string ip, string username, int uid, string actions, int medals, string reason)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@adminid", DbType.Int32,4, adminid),
				DbHelper.MakeInParam("@adminname",DbType.String,50,adminname),
                DbHelper.MakeInParam("@ip", DbType.String,15, ip),
				DbHelper.MakeInParam("@username",DbType.String,50,username),
                DbHelper.MakeInParam("@uid", DbType.Int32,4, uid),
				DbHelper.MakeInParam("@actions",DbType.String,100,actions),
                DbHelper.MakeInParam("@medals", DbType.Int32,4, medals),
				DbHelper.MakeInParam("@reason",DbType.String,100,reason)
			};
            string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "medalslog] (adminid,adminname,ip,username,uid,actions,medals,reason) VALUES (@adminid,@adminname,@ip,@username,@uid,@actions,@medals,@reason)";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public void UpdateMedalslog(string newactions, DateTime postdatetime, string reason, string oldactions, int medals, int uid)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@newactions",DbType.String,100,newactions),
                DbHelper.MakeInParam("@postdatetime",DbType.DateTime,8,postdatetime),
				DbHelper.MakeInParam("@reason",DbType.String,100,reason),
                DbHelper.MakeInParam("@oldactions",DbType.String,100,oldactions),
                DbHelper.MakeInParam("@medals", DbType.Int32,4, medals),
                DbHelper.MakeInParam("@uid", DbType.Int32,4, uid)
			};
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "medalslog] SET [actions]=@newactions ,[postdatetime]=@postdatetime, reason=@reason  WHERE [actions]=@oldactions AND [medals]=@medals  AND [uid]=@uid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public void UpdateMedalslog(string actions, DateTime postdatetime, string reason, int uid)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@actions",DbType.String,100,actions),
                DbHelper.MakeInParam("@postdatetime",DbType.DateTime,8,postdatetime),
				DbHelper.MakeInParam("@reason",DbType.String,100,reason),
                DbHelper.MakeInParam("@uid", DbType.Int32,4, uid)
			};
            string sql = "Update [" + BaseConfigs.GetTablePrefix + "medalslog] SET [actions]=@actions ,[postdatetime]=@postdatetime,[reason]=@reason  WHERE [uid]=@uid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public void UpdateMedalslog(string newactions, DateTime postdatetime, string reason, string oldactions, string medalidlist, int uid)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@newactions",DbType.String,100,newactions),
                DbHelper.MakeInParam("@postdatetime",DbType.DateTime,8,postdatetime),
				DbHelper.MakeInParam("@reason",DbType.String,100,reason),
                DbHelper.MakeInParam("@oldactions",DbType.String,100,oldactions),
                DbHelper.MakeInParam("@uid", DbType.Int32,4, uid)
			};
            string sql = "Update [" + BaseConfigs.GetTablePrefix + "medalslog] SET [actions]=@newactions ,[postdatetime]=@postdatetime, reason=@reason  WHERE [actions]=@oldactions AND [medals] NOT IN (" + medalidlist + ") AND [uid]=@uid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public void SetStopTalkUser(string uidlist)
        {
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [groupid]=4, [adminid]=0  WHERE [uid] IN (" + uidlist + ")";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql);
        }

        public void ChangeUserGroupByUid(int groupid, string uidlist)
        {
            DbParameter[] parms = 
			{
                DbHelper.MakeInParam("@groupid",DbType.Int32,4,groupid)
			};
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [groupid]=@groupid  WHERE [uid] IN (" + uidlist + ")";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        //public DataTable GetTableListInfo()
        //{
        //    string sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "tablelist]";
        //    return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        //}

        public void DeletePostByPosterid(int tabid, int posterid)
        {
            DbParameter[] parms = 
			{
                DbHelper.MakeInParam("@posterid", DbType.Int32,4, posterid)
			};
            string sql = "DELETE FROM  [" + BaseConfigs.GetTablePrefix + "posts" + tabid + "]   WHERE [posterid]=@posterid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public void DeleteTopicByPosterid(int posterid)
        {
            DbParameter[] parms = 
			{
                DbHelper.MakeInParam("@posterid", DbType.Int32,4, posterid)
			};
            string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [posterid]=@posterid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public void ClearPosts(int uid)
        {
            DbParameter[] parms = 
			{
                DbHelper.MakeInParam("@uid", DbType.Int32,4, uid)
			};
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [digestposts]=0 , [posts]=0  WHERE [uid]=@uid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public void UpdateEmailValidateInfo(string authstr, DateTime authtime, int uid)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@authstr",DbType.AnsiString,20,authstr),
                DbHelper.MakeInParam("@authtime",DbType.DateTime,8,authtime),
                DbHelper.MakeInParam("@uid", DbType.Int32,4, uid)
			};
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "userfields] SET [Authstr]=@authstr,[Authtime]=@authtime ,[Authflag]=1  WHERE [uid]=@uid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public int GetRadminidByGroupid(int groupid)
        {
            DbParameter[] parms = 
			{
                DbHelper.MakeInParam("@groupid", DbType.Int32,4, groupid)
			};
            string sql = "SELECT TOP 1 [radminid] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]=@groupid";
            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, sql, parms));
        }

        public string GetTemplateInfo()
        {
            string sql = "SELECT [templateid], [name] FROM [" + BaseConfigs.GetTablePrefix + "templates]";
            return sql;
        }

        public DataTable GetUserEmailByGroupid(string groupidlist)
        {
            string sql = "SELECT [username],[Email]  From [" + BaseConfigs.GetTablePrefix + "users] WHERE [Email] Is Not null AND [Email]<>'' AND [groupid] IN(" + groupidlist + ")";
            return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public DataTable GetUserGroupExceptGroupid(int groupid)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@groupid",DbType.Int32, 4,groupid)
			};
            string sql = "SELECT [groupid] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [radminid]=0 And [groupid]>8 AND [groupid]<>@groupid";
            return DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0];
        }

        /// <summary>
        /// 创建收藏信息
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="tid">主题ID</param>
        /// <returns>创建成功返回 1 否则返回 0</returns>	
        public int CreateFavorites(int uid, int tid)
        {
            return CreateFavorites(uid, tid, 0);
        }

        /// <summary>
        /// 创建收藏信息
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="tid">主题ID</param>
        /// <param name="type">收藏类型，0=主题，1=相册，2=博客日志</param>
        /// <returns>创建成功返回 1 否则返回 0</returns>	
        public int CreateFavorites(int uid, int tid, byte type)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid",DbType.Int32,4,uid),
									   DbHelper.MakeInParam("@tid",DbType.Int32,4,tid),
                                       DbHelper.MakeInParam("@type", DbType.Byte, 4, type)
								   };
            return DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "createfavorite", parms);
        }



        /// <summary>
        /// 删除指定用户的收藏信息
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="fitemid">要删除的收藏信息id列表,以英文逗号分割</param>
        /// <returns>删除的条数．出错时返回 -1</returns>
        public int DeleteFavorites(int uid, string fidlist, byte type)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid", DbType.Int32,4, uid),
                                       DbHelper.MakeInParam("@typeid", DbType.Byte, 1, type)
			                        };
            return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "favorites] WHERE [tid] IN (" + fidlist + ") AND [uid] = @uid AND [typeid]=@typeid", parms);
        }

        /// <summary>
        /// 得到用户收藏信息列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="pagesize">分页时每页的记录数</param>
        /// <param name="pageindex">当前页码</param>
        /// <returns>用户信息列表</returns>
        public DataTable GetFavoritesList(int uid, int pagesize, int pageindex)
        {
            return GetFavoritesList(uid, pagesize, pageindex, 0);
        }

        /// <summary>
        /// 得到用户收藏信息列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="pagesize">分页时每页的记录数</param>
        /// <param name="pageindex">当前页码</param>
        /// <param name="typeid">收藏类型id</param>
        /// <returns>用户信息列表</returns>
        public DataTable GetFavoritesList(int uid, int pagesize, int pageindex, int typeid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid",DbType.Int32,4,uid),
									   DbHelper.MakeInParam("@pagesize", DbType.Int32,4,pagesize),
									   DbHelper.MakeInParam("@pageindex",DbType.Int32,4,pageindex)
								   
								   };

            switch (typeid)
            {
                case 1:
                    return DbHelper.ExecuteDataset(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getfavoriteslistbyalbum", parms).Tables[0];

                case 2:
                    return DbHelper.ExecuteDataset(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getfavoriteslistbyspacepost", parms).Tables[0];

                case 3:
                    {   //获取收藏的商品信息
                        string strSQL = "SELECT [f].[tid], [f].[uid], [goodsid], [shopid], [categoryid] , [title] , [price], [selleruid], [seller], [dateline], [expiration]  FROM [{0}favorites] [f],[{0}goods] [goods] WHERE [f].[tid]=[goods].[goodsid] AND [f].[typeid]=3  AND [f].[uid]= " + uid;
                        
                        if(pageindex == 1)
                        {
                            strSQL = "SELECT TOP " + pagesize + "  [tid], [uid], [goodsid], [shopid], [categoryid] , [title] , [price], [selleruid] AS [posterid], [seller] AS [poster], [dateline] AS [postdatetime], [expiration]  FROM ( " + strSQL + ") f  ORDER BY [tid] DESC";
                        }
                        else
                        {
                            strSQL = "SELECT TOP " + pagesize + "  [tid], [uid], [goodsid], [shopid], [categoryid] , [title] , [price], [selleruid] AS [posterid], [seller] AS [poster], [dateline] AS [postdatetime], [expiration]  FROM ( " + @strSQL + ") f1 WHERE [tid] < (SELECT MIN([tid]) FROM (SELECT TOP " + ((pageindex - 1) * pagesize) + " [tid] FROM (" + strSQL + ") f2  ORDER BY [tid] DESC) AS tblTmp) ORDER BY [tid] DESC";
                        }

                        return DbHelper.ExecuteDataset(CommandType.Text, string.Format(strSQL, BaseConfigs.GetTablePrefix)).Tables[0];
                    }

                //case FavoriteType.ForumTopic:
                default:
                    return DbHelper.ExecuteDataset(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getfavoriteslist", parms).Tables[0];

            }
        }

        /// <summary>
        /// 得到用户收藏的总数
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>收藏总数</returns>
        public int GetFavoritesCount(int uid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid",DbType.Int32,4,uid),
								   };
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getfavoritescount", parms).ToString(), 0);
        }

        public int GetFavoritesCount(int uid, int typeid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid",DbType.Int32,4,uid),
                                       DbHelper.MakeInParam("@typeid",DbType.Byte,1,typeid)
								   };
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getfavoritescountbytype", parms).ToString(), 0);
        }

        public int CheckFavoritesIsIN(int uid, int tid)
        {
            return CreateFavorites(uid, tid, 0);
        }

        /// <summary>
        /// 收藏夹里是否包含了指定的主题
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="tid">主题id</param>
        /// <returns></returns>
        public int CheckFavoritesIsIN(int uid, int tid, byte type)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid",DbType.Int32,4,uid),
									   DbHelper.MakeInParam("@tid",DbType.Int32,4,tid),
                                        DbHelper.MakeInParam("@type", DbType.Byte, 1, type)
			};
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([tid]) AS [tidcount] FROM [" + BaseConfigs.GetTablePrefix + "favorites] WHERE [tid]=@tid AND [uid]=@uid AND [typeid]=@type", parms), 0);
        }

        public void UpdateUserShortInfo(string location, string bio, string signature, int uid)
        {
            //更新签名 location来自 bio个人介绍
            DbParameter[] parms ={
                                        DbHelper.MakeInParam("@signature",DbType.String,500,signature),
                                        DbHelper.MakeInParam("@location",DbType.String,50,location),
                                        DbHelper.MakeInParam("@bio",DbType.String,50,bio),
                                        DbHelper.MakeInParam("@uid",DbType.Int32,4,uid)

        
                                    };

            string sql = "Update [" + BaseConfigs.GetTablePrefix + "userfields] SET signature=@signature, location=@location,bio=@bio WHERE uid=@uid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }


        public void UpdateUserAllInfo(UserInfo userinfo)
        {
            string sqlstring = "Update [" + BaseConfigs.GetTablePrefix + "users] Set username=@username ,nickname=@nickname,secques=@secques,gender=@gender,adminid=@adminid,groupid=@groupid,groupexpiry=@groupexpiry,extgroupids=@extgroupids, regip=@regip," +
                "joindate=@joindate , lastip=@lastip, lastvisit=@lastvisit,  lastactivity=@lastactivity, lastpost=@lastpost, lastposttitle=@lastposttitle,posts=@posts, digestposts=@digestposts,oltime=@oltime,pageviews=@pageviews,credits=@credits," +
                "avatarshowid=@avatarshowid, email=@email,bday=@bday,sigstatus=@sigstatus,tpp=@tpp,ppp=@ppp,templateid=@templateid,pmsound=@pmsound," +
                "showemail=@showemail,newsletter=@newsletter,invisible=@invisible,newpm=@newpm,accessmasks=@accessmasks,extcredits1=@extcredits1,extcredits2=@extcredits2,extcredits3=@extcredits3,extcredits4=@extcredits4,extcredits5=@extcredits5,extcredits6=@extcredits6,extcredits7=@extcredits7,extcredits8=@extcredits8   Where uid=@uid";

            DbParameter[] parms = {
				DbHelper.MakeInParam("@username", DbType.AnsiString, 20, userinfo.Username),
				DbHelper.MakeInParam("@nickname", DbType.AnsiString, 10, userinfo.Nickname),
				DbHelper.MakeInParam("@secques", DbType.AnsiString, 8, userinfo.Secques),
				DbHelper.MakeInParam("@gender", DbType.Int32, 4, userinfo.Gender),
				DbHelper.MakeInParam("@adminid", DbType.Int32, 4, userinfo.Uid == 1 ? 1 : userinfo.Adminid),
				DbHelper.MakeInParam("@groupid", DbType.Int16, 2, userinfo.Groupid),
				DbHelper.MakeInParam("@groupexpiry", DbType.Int32, 4, userinfo.Groupexpiry),
				DbHelper.MakeInParam("@extgroupids", DbType.AnsiString, 60, userinfo.Extgroupids),
				DbHelper.MakeInParam("@regip", DbType.AnsiString, 15, userinfo.Regip),
				DbHelper.MakeInParam("@joindate", DbType.DateTime, 4, userinfo.Joindate),
				DbHelper.MakeInParam("@lastip", DbType.AnsiString, 15, userinfo.Lastip),
				DbHelper.MakeInParam("@lastvisit", DbType.DateTime, 8, userinfo.Lastvisit),
				DbHelper.MakeInParam("@lastactivity", DbType.DateTime, 8, userinfo.Lastactivity),
				DbHelper.MakeInParam("@lastpost", DbType.DateTime, 8, userinfo.Lastpost),
				DbHelper.MakeInParam("@lastposttitle", DbType.AnsiString, 80, userinfo.Lastposttitle),
				DbHelper.MakeInParam("@posts", DbType.Int32, 4, userinfo.Posts),
				DbHelper.MakeInParam("@digestposts", DbType.Int16, 2, userinfo.Digestposts),
				DbHelper.MakeInParam("@oltime", DbType.Int32, 4, userinfo.Oltime),
				DbHelper.MakeInParam("@pageviews", DbType.Int32, 4, userinfo.Pageviews),
				DbHelper.MakeInParam("@credits", DbType.Decimal, 10, userinfo.Credits),
				DbHelper.MakeInParam("@avatarshowid", DbType.Int32, 4, userinfo.Avatarshowid),
				DbHelper.MakeInParam("@email", DbType.AnsiString, 50, userinfo.Email.ToString()),
				DbHelper.MakeInParam("@bday", DbType.AnsiString, 10, userinfo.Bday.ToString()),
				DbHelper.MakeInParam("@sigstatus", DbType.Int32, 4, userinfo.Sigstatus.ToString()),
				DbHelper.MakeInParam("@tpp", DbType.Int32, 4, userinfo.Tpp),
				DbHelper.MakeInParam("@ppp", DbType.Int32, 4, userinfo.Ppp),
				DbHelper.MakeInParam("@templateid", DbType.Int32, 4, userinfo.Templateid),
				DbHelper.MakeInParam("@pmsound", DbType.Int32, 4, userinfo.Pmsound),
				DbHelper.MakeInParam("@showemail", DbType.Int32, 4, userinfo.Showemail),
				DbHelper.MakeInParam("@newsletter", DbType.Int32, 4, userinfo.Newsletter),
				DbHelper.MakeInParam("@invisible", DbType.Int32, 4, userinfo.Invisible),
				DbHelper.MakeInParam("@newpm", DbType.Int32, 4, userinfo.Newpm),
				DbHelper.MakeInParam("@accessmasks", DbType.Int32, 4, userinfo.Accessmasks),
				DbHelper.MakeInParam("@extcredits1", DbType.Decimal, 10, userinfo.Extcredits1),
				DbHelper.MakeInParam("@extcredits2", DbType.Decimal, 10, userinfo.Extcredits2),
				DbHelper.MakeInParam("@extcredits3", DbType.Decimal, 10, userinfo.Extcredits3),
				DbHelper.MakeInParam("@extcredits4", DbType.Decimal, 10, userinfo.Extcredits4),
				DbHelper.MakeInParam("@extcredits5", DbType.Decimal, 10, userinfo.Extcredits5),
				DbHelper.MakeInParam("@extcredits6", DbType.Decimal, 10, userinfo.Extcredits6),
				DbHelper.MakeInParam("@extcredits7", DbType.Decimal, 10, userinfo.Extcredits7),
				DbHelper.MakeInParam("@extcredits8", DbType.Decimal, 10, userinfo.Extcredits8),
				DbHelper.MakeInParam("@uid", DbType.Int32, 4, userinfo.Uid)
			};

            DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, parms);
        }

        public void DeleteModerator(int uid)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "moderators] WHERE [uid]=" + uid);
        }

        public void UpdateUserField(UserInfo userinfo, string signature, string authstr, string sightml)
        {
            string sqlstring = "Update [" + BaseConfigs.GetTablePrefix + "userfields] Set website=@website,icq=@icq,qq=@qq,yahoo=@yahoo,msn=@msn,skype=@skype,location=@location,customstatus=@customstatus, avatar=@avatar," +
                         "avatarwidth=@avatarwidth , avatarheight=@avatarheight, medals=@medals,  authstr=@authstr, authtime=@authtime, authflag=@authflag,bio=@bio, signature=@signature,sightml=@sightml,realname=@Realname,idcard=@Idcard,mobile=@Mobile,phone=@Phone Where uid=@uid";

            DbParameter[] prams1 = {
				DbHelper.MakeInParam("@website", DbType.String, 80, userinfo.Website),
				DbHelper.MakeInParam("@icq", DbType.AnsiString, 12, userinfo.Icq),
				DbHelper.MakeInParam("@qq", DbType.AnsiString, 12, userinfo.Qq),
				DbHelper.MakeInParam("@yahoo", DbType.AnsiString, 40, userinfo.Yahoo),
				DbHelper.MakeInParam("@msn", DbType.AnsiString, 40, userinfo.Msn),
				DbHelper.MakeInParam("@skype", DbType.AnsiString, 40, userinfo.Skype),
				DbHelper.MakeInParam("@location", DbType.String, 50, userinfo.Location),
				DbHelper.MakeInParam("@customstatus", DbType.String, 50, userinfo.Customstatus),
				DbHelper.MakeInParam("@avatar", DbType.String, 255, userinfo.Avatar),
				DbHelper.MakeInParam("@avatarwidth", DbType.Int32, 4, userinfo.Avatarwidth),
				DbHelper.MakeInParam("@avatarheight", DbType.Int32, 4, userinfo.Avatarheight),
				DbHelper.MakeInParam("@medals", DbType.AnsiString, 300, userinfo.Medals),
				DbHelper.MakeInParam("@authstr", DbType.AnsiString, 20, authstr),
				DbHelper.MakeInParam("@authtime", DbType.DateTime, 4, userinfo.Authtime),
				DbHelper.MakeInParam("@authflag", DbType.Byte, 1, 1),
				DbHelper.MakeInParam("@bio", DbType.String, 500, userinfo.Bio.ToString()),
				DbHelper.MakeInParam("@signature", DbType.String, 500, signature),
				DbHelper.MakeInParam("@sightml", DbType.String, 1000, sightml),
                 DbHelper.MakeInParam("@Realname", DbType.String, 1000, userinfo.Realname),
                DbHelper.MakeInParam("@Idcard", DbType.String, 1000, userinfo.Idcard),
                DbHelper.MakeInParam("@Mobile", DbType.String, 1000, userinfo.Mobile),
                DbHelper.MakeInParam("@Phone", DbType.String, 1000, userinfo.Phone),
				DbHelper.MakeInParam("@uid", DbType.Int32, 4, userinfo.Uid)
			};

            DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams1);
        }



        public void UpdatePMSender(int msgfromid, string msgfrom)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@msgfromid", DbType.Int32, 4, msgfromid),
                                        DbHelper.MakeInParam("@msgfrom", DbType.AnsiString, 20, msgfrom)
                                    };

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "pms] SET [msgfrom]=@msgfrom WHERE [msgfromid]=@msgfromid", parms);
        }

        public void UpdatePMReceiver(int msgtoid, string msgto)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@msgtoid", DbType.Int32, 4, msgtoid),
                                        DbHelper.MakeInParam("@msgto", DbType.AnsiString, 20, msgto)
                                    };

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "pms] SET [msgto]=@msgto  WHERE [msgtoid]=@msgtoid", parms);
        }



        public DataRowCollection GetModerators(string oldusername)
        {
            DbParameter[] parms = {
				DbHelper.MakeInParam("@oldusername", DbType.AnsiString, 20, RegEsc(oldusername))
			};

            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [fid],[moderators] FROM  [" + BaseConfigs.GetTablePrefix + "forumfields] WHERE [moderators] LIKE '% @oldusername %'", parms).Tables[0].Rows;
        }

        public DataTable GetModeratorsTable(string oldusername)
        {
            DbParameter[] parms = {
				DbHelper.MakeInParam("@oldusername", DbType.AnsiString, 20, RegEsc(oldusername))
			};

            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [fid],[moderators] FROM  [" + BaseConfigs.GetTablePrefix + "forumfields] WHERE [moderators] LIKE '% @oldusername %'", parms).Tables[0];
        }

        public void UpdateModerators(int fid, string moderators)
        {
            DbParameter[] parm = { 
                                        DbHelper.MakeInParam("@moderators", DbType.AnsiString, 20, moderators),
                                        DbHelper.MakeInParam("@fid", DbType.Int32, 4, fid)
                                    };

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "forumfields] SET [moderators]=@moderators  WHERE [fid]=@fid", parm);
        }

        public void UpdateUserCredits(int userid, float credits, float extcredits1, float extcredits2, float extcredits3, float extcredits4, float extcredits5, float extcredits6, float extcredits7, float extcredits8)
        {
            DbParameter[] prams1 = {
					DbHelper.MakeInParam("@targetuid",DbType.Int32,4,userid.ToString()),
					DbHelper.MakeInParam("@Credits",DbType.Decimal,9, credits),
					DbHelper.MakeInParam("@Extcredits1", DbType.Decimal, 20,extcredits1),
					DbHelper.MakeInParam("@Extcredits2", DbType.Decimal, 20,extcredits2),
					DbHelper.MakeInParam("@Extcredits3", DbType.Decimal, 20,extcredits3),
					DbHelper.MakeInParam("@Extcredits4", DbType.Decimal, 20,extcredits4),
					DbHelper.MakeInParam("@Extcredits5", DbType.Decimal, 20,extcredits5),
					DbHelper.MakeInParam("@Extcredits6", DbType.Decimal, 20,extcredits6),
					DbHelper.MakeInParam("@Extcredits7", DbType.Decimal, 20,extcredits7),
					DbHelper.MakeInParam("@Extcredits8", DbType.Decimal, 20,extcredits8)
										};

            string sqlstring = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET credits=@Credits,extcredits1=@Extcredits1, extcredits2=@Extcredits2, extcredits3=@Extcredits3, extcredits4=@Extcredits4, extcredits5=@Extcredits5, extcredits6=@Extcredits6, extcredits7=@Extcredits7, extcredits8=@Extcredits8 WHERE [uid]=@targetuid";

            DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams1);
        }

        public void UpdateUserCredits(int userid, int extcreditsid, float score)
        {
            DbParameter[] prams1 = {
					DbHelper.MakeInParam("@targetuid",DbType.Int32,4,userid.ToString()),
					DbHelper.MakeInParam("@Extcredits", DbType.Single, 8, score)
             };

            string sqlstring = string.Format("UPDATE [{0}users] SET extcredits{1}=extcredits{1} + @Extcredits WHERE [uid]=@targetuid", BaseConfigs.GetTablePrefix, extcreditsid);

            DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, prams1);
        }

        public void CombinationUser(string posttablename, UserInfo targetuserinfo, UserInfo srcuserinfo)
        {
            DbParameter[] parms = {
					DbHelper.MakeInParam("@target_uid", DbType.Int32, 4, targetuserinfo.Uid),
					DbHelper.MakeInParam("@target_username", DbType.AnsiString, 20, targetuserinfo.Username.Trim()),
					DbHelper.MakeInParam("@src_uid", DbType.Int32, 4, srcuserinfo.Uid)
				};

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE  [" + BaseConfigs.GetTablePrefix + "topics] SET [posterid]=@target_uid,[poster]=@target_username  WHERE [posterid]=@src_uid", parms);

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [posts]=" + (srcuserinfo.Posts + targetuserinfo.Posts) + "WHERE [uid]=@target_uid", parms);

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE  [" + posttablename + "] SET [posterid]=@target_uid,[poster]=@target_username  WHERE [posterid]=@src_uid", parms);

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE  [" + BaseConfigs.GetTablePrefix + "pms] SET [msgtoid]=@target_uid,[msgto]=@target_username  WHERE [msgtoid]=@src_uid", parms);

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE  [" + BaseConfigs.GetTablePrefix + "attachments] SET [uid]=@target_uid WHERE [uid]=@src_uid", parms);

        }

        /// <summary>
        /// 通过用户名得到UID
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public int GetuidByusername(string username)
        {
            DbParameter[] parms = {
				DbHelper.MakeInParam("@username", DbType.AnsiString, 20, username)
			};

            DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [uid] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [username]=@username", parms).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0][0].ToString());
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 删除指定用户的所有信息
        /// </summary>
        /// <param name="uid">指定的用户uid</param>
        /// <param name="delposts">是否删除帖子</param>
        /// <param name="delpms">是否删除短消息</param>
        /// <returns></returns>
        public bool DelUserAllInf(int uid, bool delposts, bool delpms)
        {
            SQLiteConnection conn = new SQLiteConnection(DbHelper.ConnectionString);
            conn.Open();
            using (SQLiteTransaction trans = conn.BeginTransaction())
            {
                try
                {
                    DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid]=" + uid.ToString());
                    DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "userfields] WHERE [uid]=" + uid.ToString());
                    DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "onlinetime] WHERE [uid]=" + uid.ToString());
                    DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "polls] WHERE [uid]=" + uid.ToString());
                    DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "favorites] WHERE [uid]=" + uid.ToString());

                    if (delposts)
                    {
                        DbHelper.ExecuteNonQuery(CommandType.Text, "Delete From [" + BaseConfigs.GetTablePrefix + "topics] Where [posterid]=" + uid.ToString());

                        //清除用户所发的帖子
                        foreach (DataRow dr in DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "tablelist]").Tables[0].Rows)
                        {
                            if (dr["id"].ToString() != "")
                            {
                                DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM  [" + BaseConfigs.GetTablePrefix + "posts" + dr["id"].ToString() + "] WHERE [posterid]=" + uid);
                            }
                        }
                    }
                    else
                    {
                        DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [poster]='该用户已被删除'  Where [posterid]=" + uid.ToString());

                        DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [lastposter]='该用户已被删除'  Where [lastpostid]=" + uid.ToString());

                        //清除用户所发的帖子
                        foreach (DataRow dr in DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "tablelist]").Tables[0].Rows)
                        {
                            if (dr["id"].ToString() != "")
                            {
                                DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE  [" + BaseConfigs.GetTablePrefix + "posts" + dr["id"].ToString() + "] SET  [poster]='该用户已被删除'  WHERE [posterid]=" + uid);
                            }
                        }
                    }

                    if (delpms)
                    {
                        DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "pms] Where [msgfromid]=" + uid.ToString());
                    }
                    else
                    {
                        DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "pms] SET [msgfrom]='该用户已被删除'  Where [msgfromid]=" + uid.ToString());
                        DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "pms] SET [msgto]='该用户已被删除'  Where [msgtoid]=" + uid.ToString());
                    }

                    //删除版主表的相关用户信息
                    DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "moderators] WHERE [uid]=" + uid.ToString());

                    //更新当前论坛总人数
                    DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "Statistics] SET [totalusers]=[totalusers]-1");

                    DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [uid],[username] FROM [" + BaseConfigs.GetTablePrefix + "users] ORDER BY [uid] DESC").Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        //更新当前论坛最新注册会员信息
                        DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "Statistics] SET [lastuserid]=" + dt.Rows[0][0] + ", [lastusername]='" + dt.Rows[0][1] + "'");
                    }



                    trans.Commit();

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
            conn.Close();
            return true;
        }

        public DataTable GetUserGroup(int groupid)
        {
            DbParameter parm = DbHelper.MakeInParam("@groupid", DbType.Int32, 4, groupid);

            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]=@groupid", parm).Tables[0];
        }

        public DataTable GetAdminGroup(int groupid)
        {
            DbParameter parm = DbHelper.MakeInParam("@groupid", DbType.Int32, 4, groupid);

            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 * FROM [" + BaseConfigs.GetTablePrefix + "admingroups] WHERE [admingid]=@groupid", parm).Tables[0];
        }

        public void AddUserGroup(UserGroupInfo usergroupinfo, int Creditshigher, int Creditslower)
        {
            DbParameter[] parms = 
					{
						DbHelper.MakeInParam("@Radminid",DbType.Int32,4,usergroupinfo.Radminid),
						DbHelper.MakeInParam("@Grouptitle",DbType.String,50, Utils.RemoveFontTag(usergroupinfo.Grouptitle)),
						DbHelper.MakeInParam("@Creditshigher",DbType.Int32,4,Creditshigher),
						DbHelper.MakeInParam("@Creditslower",DbType.Int32,4,Creditslower),
						DbHelper.MakeInParam("@Stars",DbType.Int32,4,usergroupinfo.Stars),
						DbHelper.MakeInParam("@Color",DbType.AnsiString,7,usergroupinfo.Color),
						DbHelper.MakeInParam("@Groupavatar",DbType.String,60,usergroupinfo.Groupavatar),
						DbHelper.MakeInParam("@Readaccess",DbType.Int32,4,usergroupinfo.Readaccess),
						DbHelper.MakeInParam("@Allowvisit",DbType.Int32,4,usergroupinfo.Allowvisit),
						DbHelper.MakeInParam("@Allowpost",DbType.Int32,4,usergroupinfo.Allowpost),
						DbHelper.MakeInParam("@Allowreply",DbType.Int32,4,usergroupinfo.Allowreply),
						DbHelper.MakeInParam("@Allowpostpoll",DbType.Int32,4,usergroupinfo.Allowpostpoll),
						DbHelper.MakeInParam("@Allowdirectpost",DbType.Int32,4,usergroupinfo.Allowdirectpost),
						DbHelper.MakeInParam("@Allowgetattach",DbType.Int32,4,usergroupinfo.Allowgetattach),
						DbHelper.MakeInParam("@Allowpostattach",DbType.Int32,4,usergroupinfo.Allowpostattach),
						DbHelper.MakeInParam("@Allowvote",DbType.Int32,4,usergroupinfo.Allowvote),
						DbHelper.MakeInParam("@Allowmultigroups",DbType.Int32,4,usergroupinfo.Allowmultigroups),
						DbHelper.MakeInParam("@Allowsearch",DbType.Int32,4,usergroupinfo.Allowsearch),
						DbHelper.MakeInParam("@Allowavatar",DbType.Int32,4,usergroupinfo.Allowavatar),
						DbHelper.MakeInParam("@Allowcstatus",DbType.Int32,4,usergroupinfo.Allowcstatus),
						DbHelper.MakeInParam("@Allowuseblog",DbType.Int32,4,usergroupinfo.Allowuseblog),
						DbHelper.MakeInParam("@Allowinvisible",DbType.Int32,4,usergroupinfo.Allowinvisible),
						DbHelper.MakeInParam("@Allowtransfer",DbType.Int32,4,usergroupinfo.Allowtransfer),
						DbHelper.MakeInParam("@Allowsetreadperm",DbType.Int32,4,usergroupinfo.Allowsetreadperm),
						DbHelper.MakeInParam("@Allowsetattachperm",DbType.Int32,4,usergroupinfo.Allowsetattachperm),
						DbHelper.MakeInParam("@Allowhidecode",DbType.Int32,4,usergroupinfo.Allowhidecode),
						DbHelper.MakeInParam("@Allowhtml",DbType.Int32,4,usergroupinfo.Allowhtml),
						DbHelper.MakeInParam("@Allowcusbbcode",DbType.Int32,4,usergroupinfo.Allowcusbbcode),
						DbHelper.MakeInParam("@Allownickname",DbType.Int32,4,usergroupinfo.Allownickname),
						DbHelper.MakeInParam("@Allowsigbbcode",DbType.Int32,4,usergroupinfo.Allowsigbbcode),
						DbHelper.MakeInParam("@Allowsigimgcode",DbType.Int32,4,usergroupinfo.Allowsigimgcode),
						DbHelper.MakeInParam("@Allowviewpro",DbType.Int32,4,usergroupinfo.Allowviewpro),
						DbHelper.MakeInParam("@Allowviewstats",DbType.Int32,4,usergroupinfo.Allowviewstats),
                        DbHelper.MakeInParam("@Allowtrade",DbType.Int32,4,usergroupinfo.Allowtrade),
                        DbHelper.MakeInParam("@Allowdiggs",DbType.Int32,4,usergroupinfo.Allowdiggs),

                        DbHelper.MakeInParam("@Allowdebate",DbType.Int32,4,usergroupinfo.Allowdebate),
                        DbHelper.MakeInParam("@Allowbonus",DbType.Int32,4,usergroupinfo.Allowbonus),
                        DbHelper.MakeInParam("@Minbonusprice",DbType.Int32,4,usergroupinfo.Minbonusprice),
                        DbHelper.MakeInParam("@Maxbonusprice",DbType.Int32,4,usergroupinfo.Maxbonusprice),

						DbHelper.MakeInParam("@Disableperiodctrl",DbType.Int32,4,usergroupinfo.Disableperiodctrl),
						DbHelper.MakeInParam("@Reasonpm",DbType.Int32,4,usergroupinfo.Reasonpm),
						DbHelper.MakeInParam("@Maxprice",DbType.Int16,2,usergroupinfo.Maxprice),
						DbHelper.MakeInParam("@Maxpmnum",DbType.Int16,2,usergroupinfo.Maxpmnum),
						DbHelper.MakeInParam("@Maxsigsize",DbType.Int16,2,usergroupinfo.Maxsigsize),
						DbHelper.MakeInParam("@Maxattachsize",DbType.Int32,4,usergroupinfo.Maxattachsize),
						DbHelper.MakeInParam("@Maxsizeperday",DbType.Int32,4,usergroupinfo.Maxsizeperday),
						DbHelper.MakeInParam("@Attachextensions",DbType.AnsiString,100,usergroupinfo.Attachextensions),
                        DbHelper.MakeInParam("@Maxspaceattachsize",DbType.Int32,4,usergroupinfo.Maxspaceattachsize),
                        DbHelper.MakeInParam("@Maxspacephotosize",DbType.Int32,4,usergroupinfo.Maxspacephotosize),
						DbHelper.MakeInParam("@Raterange",DbType.AnsiString,100,usergroupinfo.Raterange)
					};

            string sqlstring = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "usergroups]  ([radminid],[grouptitle],[creditshigher],[creditslower]," +
                "[stars] ,[color], [groupavatar],[readaccess], [allowvisit],[allowpost],[allowreply]," +
                "[allowpostpoll], [allowdirectpost],[allowgetattach],[allowpostattach],[allowvote],[allowmultigroups]," +
                "[allowsearch],[allowavatar],[allowcstatus],[allowuseblog],[allowinvisible],[allowtransfer]," +
                "[allowsetreadperm],[allowsetattachperm],[allowhidecode],[allowhtml],[allowcusbbcode],[allownickname]," +
                "[allowsigbbcode],[allowsigimgcode],[allowviewpro],[allowviewstats],[allowtrade],[allowdiggs],[disableperiodctrl],[reasonpm]," +
                "[maxprice],[maxpmnum],[maxsigsize],[maxattachsize],[maxsizeperday],[attachextensions],[raterange],[maxspaceattachsize]," +
                "[maxspacephotosize],[allowdebate],[allowbonus],[minbonusprice],[maxbonusprice]) VALUES(" +
                "@Radminid,@Grouptitle,@Creditshigher,@Creditslower,@Stars,@Color,@Groupavatar,@Readaccess,@Allowvisit,@Allowpost,@Allowreply," +
                "@Allowpostpoll,@Allowdirectpost,@Allowgetattach,@Allowpostattach,@Allowvote,@Allowmultigroups,@Allowsearch,@Allowavatar,@Allowcstatus," +
                "@Allowuseblog,@Allowinvisible,@Allowtransfer,@Allowsetreadperm,@Allowsetattachperm,@Allowhidecode,@Allowhtml,@Allowcusbbcode,@Allownickname," +
                "@Allowsigbbcode,@Allowsigimgcode,@Allowviewpro,@Allowviewstats,@Allowtrade,@Allowdiggs,@Disableperiodctrl,@Reasonpm,@Maxprice,@Maxpmnum,@Maxsigsize,@Maxattachsize," +
                "@Maxsizeperday,@Attachextensions,@Raterange,@Maxspaceattachsize,@Maxspacephotosize,@Allowdebate,@Allowbonus,@Minbonusprice,@Maxbonusprice)";

            DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, parms);
        }

        public void AddOnlineList(string grouptitle)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@groupid", DbType.Int32, 4, GetMaxUserGroupId()),
                                        DbHelper.MakeInParam("@title", DbType.String, 50, grouptitle)
                                    };
            string sqlstring = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "onlinelist] ([groupid], [title], [img]) VALUES(@groupid,@title, '')";

            DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, parms);
        }

        public DataTable GetMinCreditHigher()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT MIN(Creditshigher) FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]>8 AND [radminid]=0 ").Tables[0];
        }

        public DataTable GetMaxCreditLower()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT MAX(Creditslower) FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]>8 AND [radminid]=0 ").Tables[0];
        }

        public DataTable GetUserGroupByCreditshigher(int Creditshigher)
        {
            DbParameter parm = DbHelper.MakeInParam("@Creditshigher", DbType.Int32, 4, Creditshigher);
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [groupid],[creditshigher],[creditslower] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]>8 AND [radminid]=0  AND [Creditshigher]<=@Creditshigher AND @Creditshigher<[Creditslower]", parm).Tables[0];
        }

        public void UpdateUserGroupCreditsHigher(int currentGroupID, int Creditslower)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@groupid", DbType.Int32, 4, currentGroupID),
                                        DbHelper.MakeInParam("@creditshigher", DbType.Int32, 4, Creditslower)
                                    };
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "usergroups] SET creditshigher=@creditshigher WHERE [groupid]=@groupid", parms);
        }

        public void UpdateUserGroupCreidtsLower(int currentCreditsHigher, int Creditshigher)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@creditslower", DbType.Int32, 4, Creditshigher),
                                        DbHelper.MakeInParam("@creditshigher", DbType.Int32, 4, currentCreditsHigher)
                                    };
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "usergroups] SET [creditslower]=@creditslower WHERE [groupid]>8 AND [radminid]=0 AND [creditshigher]=@creditshigher", parms);
        }

        public DataTable GetUserGroupByCreditsHigherAndLower(int Creditshigher, int Creditslower)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@Creditshigher", DbType.Int32, 4, Creditshigher),
                                        DbHelper.MakeInParam("@Creditslower", DbType.Int32, 4, Creditslower)
                                    };
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [groupid] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]>8 AND [radminid]=0 AND [Creditshigher]=@Creditshigher AND [Creditslower]=@Creditslower", parms).Tables[0];
        }
        public int GetGroupCountByCreditsLower(int Creditshigher)
        {
            DbParameter parm = DbHelper.MakeInParam("@creditslower", DbType.Int32, 4, Creditshigher);
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [groupid] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]>8 AND [radminid]=0 AND [creditslower]=@creditslower", parm).Tables[0].Rows.Count;
        }

        public void UpdateUserGroupsCreditsLowerByCreditsLower(int Creditslower, int Creditshigher)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@Creditshigher", DbType.Int32, 4, Creditshigher),
                                        DbHelper.MakeInParam("@Creditslower", DbType.Int32, 4, Creditslower)
                                    };
            DbHelper.ExecuteDataset(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "usergroups] SET [creditslower]=@Creditslower WHERE [groupid]>8 AND [radminid]=0 AND [creditslower]=@Creditshigher", parms);
        }

        public void UpdateUserGroupTitleAndCreditsByGroupid(int groupid, string grouptitle, int creditslower, int creditshigher)
        {
            DbParameter[] parms = {
                DbHelper.MakeInParam("@groupid",DbType.Int32,4,groupid),
                DbHelper.MakeInParam("@grouptitle",DbType.String,50,grouptitle),
                DbHelper.MakeInParam("@creditslower",DbType.Int32,4,creditslower),
                DbHelper.MakeInParam("@creditshigher",DbType.Int32,4,creditshigher)
            };
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "usergroups] SET [grouptitle]=@grouptitle,[creditshigher]=@creditshigher,[creditslower]=@creditslower WHERE [groupid]=@groupid";
            DbHelper.ExecuteDataset(CommandType.Text, sql, parms);
        }

        public void UpdateUserGroupsCreditsHigherByCreditsHigher(int Creditshigher, int Creditslower)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@Creditshigher", DbType.Int32, 4, Creditshigher),
                                        DbHelper.MakeInParam("@Creditslower", DbType.Int32, 4, Creditslower)
                                    };

            DbHelper.ExecuteDataset(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "usergroups] SET [Creditshigher]=@Creditshigher WHERE [groupid]>8 AND [radminid]=0 AND [Creditshigher]=@Creditslower", parms);
        }

        public DataTable GetUserGroupCreditsLowerAndHigher(int groupid)
        {
            DbParameter parm = DbHelper.MakeInParam("@groupid", DbType.Int32, 4, groupid);

            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [groupid],[creditshigher],[creditslower] FROM [" + BaseConfigs.GetTablePrefix + "usergroups]  WHERE [groupid]=@groupid", parm).Tables[0];
        }

        public void UpdateUserGroup(UserGroupInfo usergroupinfo, int Creditshigher, int Creditslower)
        {
            DbParameter[] parms = 
					{
						DbHelper.MakeInParam("@Radminid",DbType.Int32,4,(usergroupinfo.Groupid == 1) ? 1 : usergroupinfo.Radminid),
						DbHelper.MakeInParam("@Grouptitle",DbType.String,50, Utils.RemoveFontTag(usergroupinfo.Grouptitle)),
						DbHelper.MakeInParam("@Creditshigher",DbType.Int32,4,Creditshigher),
						DbHelper.MakeInParam("@Creditslower",DbType.Int32,4,Creditslower),
						DbHelper.MakeInParam("@Stars",DbType.Int32,4,usergroupinfo.Stars),
						DbHelper.MakeInParam("@Color",DbType.AnsiString,7,usergroupinfo.Color),
						DbHelper.MakeInParam("@Groupavatar",DbType.String,60,usergroupinfo.Groupavatar),
						DbHelper.MakeInParam("@Readaccess",DbType.Int32,4,usergroupinfo.Readaccess),
						DbHelper.MakeInParam("@Allowvisit",DbType.Int32,4,usergroupinfo.Allowvisit),
						DbHelper.MakeInParam("@Allowpost",DbType.Int32,4,usergroupinfo.Allowpost),
						DbHelper.MakeInParam("@Allowreply",DbType.Int32,4,usergroupinfo.Allowreply),
						DbHelper.MakeInParam("@Allowpostpoll",DbType.Int32,4,usergroupinfo.Allowpostpoll),
						DbHelper.MakeInParam("@Allowdirectpost",DbType.Int32,4,usergroupinfo.Allowdirectpost),
						DbHelper.MakeInParam("@Allowgetattach",DbType.Int32,4,usergroupinfo.Allowgetattach),
						DbHelper.MakeInParam("@Allowpostattach",DbType.Int32,4,usergroupinfo.Allowpostattach),
						DbHelper.MakeInParam("@Allowvote",DbType.Int32,4,usergroupinfo.Allowvote),
						DbHelper.MakeInParam("@Allowmultigroups",DbType.Int32,4,usergroupinfo.Allowmultigroups),
						DbHelper.MakeInParam("@Allowsearch",DbType.Int32,4,usergroupinfo.Allowsearch),
						DbHelper.MakeInParam("@Allowavatar",DbType.Int32,4,usergroupinfo.Allowavatar),
						DbHelper.MakeInParam("@Allowcstatus",DbType.Int32,4,usergroupinfo.Allowcstatus),
						DbHelper.MakeInParam("@Allowuseblog",DbType.Int32,4,usergroupinfo.Allowuseblog),
						DbHelper.MakeInParam("@Allowinvisible",DbType.Int32,4,usergroupinfo.Allowinvisible),
						DbHelper.MakeInParam("@Allowtransfer",DbType.Int32,4,usergroupinfo.Allowtransfer),
						DbHelper.MakeInParam("@Allowsetreadperm",DbType.Int32,4,usergroupinfo.Allowsetreadperm),
						DbHelper.MakeInParam("@Allowsetattachperm",DbType.Int32,4,usergroupinfo.Allowsetattachperm),
						DbHelper.MakeInParam("@Allowhidecode",DbType.Int32,4,usergroupinfo.Allowhidecode),
						DbHelper.MakeInParam("@Allowhtml",DbType.Int32,4,usergroupinfo.Allowhtml),
						DbHelper.MakeInParam("@Allowcusbbcode",DbType.Int32,4,usergroupinfo.Allowcusbbcode),
						DbHelper.MakeInParam("@Allownickname",DbType.Int32,4,usergroupinfo.Allownickname),
						DbHelper.MakeInParam("@Allowsigbbcode",DbType.Int32,4,usergroupinfo.Allowsigbbcode),
						DbHelper.MakeInParam("@Allowsigimgcode",DbType.Int32,4,usergroupinfo.Allowsigimgcode),
						DbHelper.MakeInParam("@Allowviewpro",DbType.Int32,4,usergroupinfo.Allowviewpro),
						DbHelper.MakeInParam("@Allowviewstats",DbType.Int32,4,usergroupinfo.Allowviewstats),
                        DbHelper.MakeInParam("@Allowtrade",DbType.Int32,4,usergroupinfo.Allowtrade),
                        DbHelper.MakeInParam("@Allowdiggs",DbType.Int32,4,usergroupinfo.Allowdiggs),
						DbHelper.MakeInParam("@Disableperiodctrl",DbType.Int32,4,usergroupinfo.Disableperiodctrl),

                        DbHelper.MakeInParam("@Allowdebate",DbType.Int32,4,usergroupinfo.Allowdebate),
                        DbHelper.MakeInParam("@Allowbonus",DbType.Int32,4,usergroupinfo.Allowbonus),
                        DbHelper.MakeInParam("@Minbonusprice",DbType.Int32,4,usergroupinfo.Minbonusprice),
                        DbHelper.MakeInParam("@Maxbonusprice",DbType.Int32,4,usergroupinfo.Maxbonusprice),

						DbHelper.MakeInParam("@Reasonpm",DbType.Int32,4,usergroupinfo.Reasonpm),
						DbHelper.MakeInParam("@Maxprice",DbType.Int16,2,usergroupinfo.Maxprice),
						DbHelper.MakeInParam("@Maxpmnum",DbType.Int16,2,usergroupinfo.Maxpmnum),
						DbHelper.MakeInParam("@Maxsigsize",DbType.Int16,2,usergroupinfo.Maxsigsize),
						DbHelper.MakeInParam("@Maxattachsize",DbType.Int32,4,usergroupinfo.Maxattachsize),
						DbHelper.MakeInParam("@Maxsizeperday",DbType.Int32,4,usergroupinfo.Maxsizeperday),
						DbHelper.MakeInParam("@Attachextensions",DbType.AnsiString,100,usergroupinfo.Attachextensions),
                        DbHelper.MakeInParam("@Maxspaceattachsize",DbType.Int32,4,usergroupinfo.Maxspaceattachsize),
                        DbHelper.MakeInParam("@Maxspacephotosize",DbType.Int32,4,usergroupinfo.Maxspacephotosize),
						DbHelper.MakeInParam("@Groupid",DbType.Int32,4,usergroupinfo.Groupid)

					};

            string sqlstring = "UPDATE [" + BaseConfigs.GetTablePrefix + "usergroups]  SET [radminid]=@Radminid,[grouptitle]=@Grouptitle,[creditshigher]=@Creditshigher," +
                "[creditslower]=@Creditslower,[stars]=@Stars,[color]=@Color,[groupavatar]=@Groupavatar,[readaccess]=@Readaccess, [allowvisit]=@Allowvisit,[allowpost]=@Allowpost," +
                "[allowreply]=@Allowreply,[allowpostpoll]=@Allowpostpoll, [allowdirectpost]=@Allowdirectpost,[allowgetattach]=@Allowgetattach,[allowpostattach]=@Allowpostattach," +
                "[allowvote]=@Allowvote,[allowmultigroups]=@Allowmultigroups,[allowsearch]=@Allowsearch,[allowavatar]=@Allowavatar,[allowcstatus]=@Allowcstatus," +
                "[allowuseblog]=@Allowuseblog,[allowinvisible]=@Allowinvisible,[allowtransfer]=@Allowtransfer,[allowsetreadperm]=@Allowsetreadperm," +
                "[allowsetattachperm]=@Allowsetattachperm,[allowhidecode]=@Allowhidecode,[allowhtml]=@Allowhtml,[allowcusbbcode]=@Allowcusbbcode,[allownickname]=@Allownickname," +
                "[allowsigbbcode]=@Allowsigbbcode,[allowsigimgcode]=@Allowsigimgcode,[allowviewpro]=@Allowviewpro,[allowviewstats]=@Allowviewstats,[allowtrade]=@Allowtrade," +
                "[allowdiggs]=@Allowdiggs,[disableperiodctrl]=@Disableperiodctrl,[allowdebate]=@Allowdebate,[allowbonus]=@Allowbonus,[minbonusprice]=@Minbonusprice,[maxbonusprice]=@Maxbonusprice," +
                "[reasonpm]=@Reasonpm,[maxprice]=@Maxprice,[maxpmnum]=@Maxpmnum,[maxsigsize]=@Maxsigsize,[maxattachsize]=@Maxattachsize," +
                "[maxsizeperday]=@Maxsizeperday,[attachextensions]=@Attachextensions,[maxspaceattachsize]=@Maxspaceattachsize,[maxspacephotosize]=@Maxspacephotosize  WHERE [groupid]=@Groupid";


            DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, parms);
        }


        public void UpdateOnlineList(UserGroupInfo usergroupinfo)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@groupid", DbType.Int32, 4, usergroupinfo.Groupid),
                                        DbHelper.MakeInParam("@title", DbType.String, 50, Utils.RemoveFontTag(usergroupinfo.Grouptitle))
                                    };
            string sqlstring = "UPDATE [" + BaseConfigs.GetTablePrefix + "onlinelist] SET [title]=@title WHERE [groupid]=@groupid";

            DbHelper.ExecuteNonQuery(CommandType.Text, sqlstring, parms);
        }

        public bool IsSystemOrTemplateUserGroup(int groupid)
        {
            DbParameter parm = DbHelper.MakeInParam("@groupid", DbType.Int32, 4, groupid);
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 *  FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE ([system]=1 OR [type]=1) AND [groupid]=@groupid", parm).Tables[0].Rows.Count > 0;
        }

        public DataTable GetOthersCommonUserGroup(int exceptgroupid)
        {
            DbParameter parm = DbHelper.MakeInParam("@groupid", DbType.Int32, 4, exceptgroupid);
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [groupid] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [radminid]=0 And [groupid]>8 AND [groupid]<>@groupid", parm).Tables[0];
        }

        public string GetUserGroupRAdminId(int groupid)
        {
            DbParameter parm = DbHelper.MakeInParam("@groupid", DbType.Int32, 4, groupid);
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [radminid] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE  [groupid]=@groupid", parm).Tables[0].Rows[0][0].ToString();
        }

        public void UpdateUserGroupLowerAndHigherToLimit(int groupid)
        {
            DbParameter parm = DbHelper.MakeInParam("@groupid", DbType.Int32, 4, groupid);
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "usergroups] SET [creditshigher]=-9999999 ,creditslower=9999999  WHERE [groupid]=@groupid", parm);
        }

        public void DeleteUserGroup(int groupid)
        {
            DbParameter parm = DbHelper.MakeInParam("@groupid", DbType.Int32, 4, groupid);
            DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]=@groupid", parm);
        }

        public void DeleteAdminGroup(int groupid)
        {
            DbParameter parm = DbHelper.MakeInParam("@groupid", DbType.Int32, 4, groupid);
            DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "admingroups] WHERE [admingid]=@groupid", parm);
        }

        public void DeleteOnlineList(int groupid)
        {
            DbParameter parm = DbHelper.MakeInParam("@groupid", DbType.Int32, 4, groupid);
            DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "onlinelist] WHERE [groupid]=@groupid", parm);
        }

        public int GetMaxUserGroupId()
        {
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT ISNULL(MAX(groupid), 0) FROM " + BaseConfigs.GetTablePrefix + "usergroups"), 0);
        }



        public bool DeletePaymentLog()
        {
            try
            {
                DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] ");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 按指定条件删除日志
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public bool DeletePaymentLog(string condition)
        {
            try
            {
                DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] WHERE " + condition);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public DataTable GetPaymentLogList(int pagesize, int currentpage)
        {
            int pagetop = (currentpage - 1) * pagesize;
            string sqlstring;
            if (currentpage == 1)
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + " " + BaseConfigs.GetTablePrefix + "paymentlog.*, " + BaseConfigs.GetTablePrefix + "topics.fid AS fid ," + BaseConfigs.GetTablePrefix + "topics.postdatetime AS postdatetime ," + BaseConfigs.GetTablePrefix + "topics.poster AS authorname, " + BaseConfigs.GetTablePrefix + "topics.title AS title," + BaseConfigs.GetTablePrefix + "users.username As UserName  FROM " + BaseConfigs.GetTablePrefix + "paymentlog LEFT OUTER JOIN " + BaseConfigs.GetTablePrefix + "topics ON " + BaseConfigs.GetTablePrefix + "paymentlog.tid = " + BaseConfigs.GetTablePrefix + "topics.tid LEFT OUTER JOIN " + BaseConfigs.GetTablePrefix + "users ON " + BaseConfigs.GetTablePrefix + "users.uid = " + BaseConfigs.GetTablePrefix + "paymentlog.uid ORDER BY " + BaseConfigs.GetTablePrefix + "paymentlog.id DESC";
            }
            else
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + " " + BaseConfigs.GetTablePrefix + "paymentlog.*, " + BaseConfigs.GetTablePrefix + "topics.fid AS fid ," + BaseConfigs.GetTablePrefix + "topics.postdatetime AS postdatetime ," + BaseConfigs.GetTablePrefix + "topics.poster AS authorname, " + BaseConfigs.GetTablePrefix + "topics.title AS title," + BaseConfigs.GetTablePrefix + "users.username As UserName  FROM " + BaseConfigs.GetTablePrefix + "paymentlog LEFT OUTER JOIN " + BaseConfigs.GetTablePrefix + "topics ON " + BaseConfigs.GetTablePrefix + "paymentlog.tid = " + BaseConfigs.GetTablePrefix + "topics.tid LEFT OUTER JOIN " + BaseConfigs.GetTablePrefix + "users ON " + BaseConfigs.GetTablePrefix + "users.uid = " + BaseConfigs.GetTablePrefix + "paymentlog.uid WHERE [id] < (SELECT min([id])  FROM (SELECT TOP " + pagetop + " [id] FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] ORDER BY [id] DESC) AS tblTmp )  ORDER BY " + BaseConfigs.GetTablePrefix + "paymentlog.id DESC";
            }

            return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
        }

        public DataTable GetPaymentLogList(int pagesize, int currentpage, string condition)
        {
            int pagetop = (currentpage - 1) * pagesize;
            string sqlstring;
            if (currentpage == 1)
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + " " + BaseConfigs.GetTablePrefix + "paymentlog.*, " + BaseConfigs.GetTablePrefix + "topics.fid AS fid ," + BaseConfigs.GetTablePrefix + "topics.postdatetime AS postdatetime ," + BaseConfigs.GetTablePrefix + "topics.poster AS authorname, " + BaseConfigs.GetTablePrefix + "topics.title AS title," + BaseConfigs.GetTablePrefix + "users.username As UserName  FROM " + BaseConfigs.GetTablePrefix + "paymentlog LEFT OUTER JOIN " + BaseConfigs.GetTablePrefix + "topics ON " + BaseConfigs.GetTablePrefix + "paymentlog.tid = " + BaseConfigs.GetTablePrefix + "topics.tid LEFT OUTER JOIN " + BaseConfigs.GetTablePrefix + "users ON " + BaseConfigs.GetTablePrefix + "users.uid = " + BaseConfigs.GetTablePrefix + "paymentlog.uid WHERE " + condition + "  Order by [id] DESC";
            }
            else
            {
                sqlstring = "SELECT TOP " + pagesize.ToString() + " " + BaseConfigs.GetTablePrefix + "paymentlog.*, " + BaseConfigs.GetTablePrefix + "topics.fid AS fid ," + BaseConfigs.GetTablePrefix + "topics.postdatetime AS postdatetime ," + BaseConfigs.GetTablePrefix + "topics.poster AS authorname, " + BaseConfigs.GetTablePrefix + "topics.title AS title," + BaseConfigs.GetTablePrefix + "users.username As UserName  FROM " + BaseConfigs.GetTablePrefix + "paymentlog LEFT OUTER JOIN " + BaseConfigs.GetTablePrefix + "topics ON " + BaseConfigs.GetTablePrefix + "paymentlog.tid = " + BaseConfigs.GetTablePrefix + "topics.tid LEFT OUTER JOIN " + BaseConfigs.GetTablePrefix + "users ON " + BaseConfigs.GetTablePrefix + "users.uid = " + BaseConfigs.GetTablePrefix + "paymentlog.uid  WHERE [id] < (SELECT min([id])  FROM (SELECT TOP " + pagetop + " [id] FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] WHERE " + condition + " ORDER BY [id] DESC) AS tblTmp ) AND " + condition + " ORDER BY [" + BaseConfigs.GetTablePrefix + "paymentlog].[id] DESC";
            }

            return DbHelper.ExecuteDataset(CommandType.Text, sqlstring).Tables[0];
        }

        /// <summary>
        /// 得到积分交易日志记录数
        /// </summary>
        /// <returns></returns>
        public int GetPaymentLogListCount()
        {
            return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT count(id) FROM [" + BaseConfigs.GetTablePrefix + "paymentlog]").Tables[0].Rows[0][0].ToString());
        }

        /// <summary>
        /// 得到指定查询条件下的积分交易日志数
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public int GetPaymentLogListCount(string condition)
        {
            return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT count(id) FROM [" + BaseConfigs.GetTablePrefix + "paymentlog] WHERE " + condition).Tables[0].Rows[0][0].ToString());
        }

        public void DeleteModeratorByFid(int fid)
        {
            DbParameter[] parms = { DbHelper.MakeInParam("@fid", DbType.Int32, 4, fid) };
            string sql = "DELETE FROM [" + BaseConfigs.GetTablePrefix + "moderators] WHERE [fid]=@fid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }



        public DataTable GetUidModeratorByFid(string fidlist)
        {
            string sql = "SELECT distinct [uid] FROM [" + BaseConfigs.GetTablePrefix + "moderators] WHERE [fid] IN(" + fidlist + ")";
            return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public void AddModerator(int uid, int fid, int displayorder, int inherited)
        {
            DbParameter[] parms = 
            {
                DbHelper.MakeInParam("@uid", DbType.Int32, 4, uid),
                DbHelper.MakeInParam("@fid", DbType.Int16, 2, fid),
                DbHelper.MakeInParam("@displayorder", DbType.Int16, 2, displayorder),
                DbHelper.MakeInParam("@inherited", DbType.Int16, 2, inherited)
		    };
            string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "moderators] (uid,fid,displayorder,inherited) VALUES(@uid,@fid,@displayorder,@inherited)";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public DataTable GetModeratorInfo(string moderator)
        {
            DbParameter[] parms = 
            {
				DbHelper.MakeInParam("@username", DbType.AnsiString, 20, moderator.Trim())
			};

            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [uid],[groupid]  FROM [" + BaseConfigs.GetTablePrefix + "users] Where [groupid]<>7 AND [groupid]<>8 AND [username]=@username", parms).Tables[0];
        }

        public void SetModerator(string moderator)
        {
            DbParameter[] parms = 
            {
				DbHelper.MakeInParam("@username", DbType.AnsiString, 20, moderator.Trim())
			};
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [adminid]=3,[groupid]=3 WHERE [username]=@username", parms);
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "online] SET [adminid]=3,[groupid]=3 WHERE [username]=@username", parms);
        }



        public DataTable GetUidAdminIdByUsername(string username)
        {
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("@username", DbType.AnsiString, 20, username)
			};
            string sql = "SELECT TOP 1 [uid],[adminid] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [username] = @username";
            return DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0];
        }

        public DataTable GetUidInModeratorsByUid(int currentfid, int uid)
        {
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("@currentfid", DbType.Int32, 4, currentfid),
                DbHelper.MakeInParam("@uid", DbType.Int32, 4, uid)
			};
            string sql = "SELECT TOP 1 [uid]  FROM [" + BaseConfigs.GetTablePrefix + "moderators] WHERE [fid]<>@currentfid AND [uid]=@uid";
            return DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0];
        }

        public void UpdateUserOnlineInfo(int groupid, int userid)
        {
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("@groupid", DbType.Int32, 4, groupid),
                DbHelper.MakeInParam("@userid", DbType.Int32, 4, userid)
			};
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "online] SET [groupid]=@groupid WHERE [userid]=@userid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public void UpdateUserOtherInfo(int groupid, int userid)
        {
            DbParameter[] parms =
			{
				DbHelper.MakeInParam("@groupid", DbType.Int32, 4, groupid),
                DbHelper.MakeInParam("@userid", DbType.Int32, 4, userid)
			};
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [groupid]=@groupid ,[adminid]=0 WHERE [uid]=@userid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        /// <summary>
        /// 得到论坛中最后注册的用户ID和用户名
        /// </summary>
        /// <param name="lastuserid">输出参数：最后注册的用户ID</param>
        /// <param name="lastusername">输出参数：最后注册的用户名</param>
        /// <returns>存在返回true,不存在返回false</returns>
        public bool GetLastUserInfo(out string lastuserid, out string lastusername)
        {
            lastuserid = "";
            lastusername = "";

            IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 [uid],[username] FROM [" + BaseConfigs.GetTablePrefix + "users] ORDER BY [uid] DESC");
            if (reader.Read())
            {
                lastuserid = reader["uid"].ToString();
                lastusername = reader["username"].ToString().Trim();
                reader.Close();
                return true;
            }
            reader.Close();
            return false;

        }

        public IDataReader GetTopUsers(int statcount, int lastuid)
        {
            DbParameter[] parms = {
				DbHelper.MakeInParam("@lastuid", DbType.Int32, 4, lastuid),
			};

            return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP " + statcount + " [uid] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid] > @lastuid", parms);
        }

        public void ResetUserDigestPosts(int userid)
        {
            DbParameter parm = DbHelper.MakeInParam("@uid", DbType.Int32, 4, userid);
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [digestposts]=(SELECT COUNT(tid) AS [digestposts] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [" + BaseConfigs.GetTablePrefix + "topics].[posterid] = [" + BaseConfigs.GetTablePrefix + "users].[uid] AND [digest] > 0) WHERE [" + BaseConfigs.GetTablePrefix + "users].[uid] = @uid", parm);
        }

        public IDataReader GetUsers(int start_uid, int end_uid)
        {
            DbParameter[] parms = {
				DbHelper.MakeInParam("@start_uid", DbType.Int32, 4, start_uid),
				DbHelper.MakeInParam("@end_uid", DbType.Int32, 4, end_uid)
			};

            return DbHelper.ExecuteReader(CommandType.Text, "SELECT [uid] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid] >= @start_uid AND [uid]<=@end_uid", parms);
        }

        public void UpdateUserPostCount(int postcount, int userid)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@postcount", DbType.Int32, 4, postcount),
                                        DbHelper.MakeInParam("@userid", DbType.Int32, 4, userid)
                                    };
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [posts]=@postcount WHERE [" + BaseConfigs.GetTablePrefix + "users].[uid] = @userid", parms);
        }


        /// <summary>
        /// 获得所有版主列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetModeratorList()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT * FROM [{0}moderators]", BaseConfigs.GetTablePrefix)).Tables[0];
        }


        /// <summary>
        /// 获得全部在线用户数
        /// </summary>
        /// <returns></returns>
        public int GetOnlineAllUserCount()
        {
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(olid) FROM [" + BaseConfigs.GetTablePrefix + "online]"), 1);
        }

        /// <summary>
        /// 创建在线表
        /// </summary>
        /// <returns></returns>
        public int CreateOnlineTable()
        {
            System.Diagnostics.Debug.WriteLine("CreateOnlineTable()");
            try
            {
                return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("TRUNCATE TABLE [{0}online]", BaseConfigs.GetTablePrefix));
            }
            catch
            {
                return -1;
            }
        }

        ///// <summary>
        ///// 取得在线表最后一条记录的tickcount字段
        ///// </summary>
        ///// <returns></returns>
        //public DateTime GetLastUpdateTime()
        //{
        //    object val =
        //        DbHelper.ExecuteScalar(CommandType.Text,
        //                               "SELECT TOP 1 [lastupdatetime] FROM [" + BaseConfigs.GetTablePrefix +
        //                               "online] ORDER BY [olid] DESC");
        //    return val == null ? DateTime.Now : Convert.ToDateTime(val);
        //}

        /// <summary>
        /// 获得在线注册用户总数量
        /// </summary>
        /// <returns>用户数量</returns>
        public int GetOnlineUserCount()
        {

            return (int)DbHelper.ExecuteDataset(CommandType.Text, "SELECT COUNT(olid) FROM [" + BaseConfigs.GetTablePrefix + "online] WHERE [userid]>0").Tables[0].Rows[0][0];

        }

        /// <summary>
        /// 获得版块在线用户列表
        /// </summary>
        /// <param name="forumid">版块Id</param>
        /// <returns></returns>
        public DataTable GetForumOnlineUserListTable(int forumid)
        {
            return DbHelper.ExecuteDataset(CommandType.StoredProcedure, string.Format("{0}getonlineuserlistbyfid", BaseConfigs.GetTablePrefix), DbHelper.MakeInParam("@fid", DbType.Int32, 4, forumid)).Tables[0];
        }

        /// <summary>
        /// 获得全部在线用户列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetOnlineUserListTable()
        {
            return DbHelper.ExecuteDataset(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "getonlineuserlist").Tables[0];
        }

        /// <summary>
        /// 获得版块在线用户列表
        /// </summary>
        /// <param name="forumid">版块Id</param>
        /// <returns></returns>
        public IDataReader GetForumOnlineUserList(int forumid)
        {
            string sql = string.Format("SELECT `olid`,`userid`,`ip`,`username`,`nickname`,`password`,`groupid`,`olimg`,`adminid`,`invisible`,`action`,`lastactivity`,`lastposttime`,`lastpostpmtime`,`lastsearchtime`,`lastupdatetime`,`forumid`,`forumname`,`titleid`,`title`,`verifycode`,`newpms`,`newnotices` FROM `dnt_online` WHERE `forumid`=@fid", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, sql, DbHelper.MakeInParam("@fid", DbType.Int32, 4, forumid));
        }

        /// <summary>
        /// 获得全部在线用户列表
        /// </summary>
        /// <returns></returns>
        public IDataReader GetOnlineUserList()
        {
            System.Diagnostics.Debug.WriteLine("GetOnlineUserList()");
            string sql = string.Format("SELECT [olid],[userid],[ip],[username],[nickname],[password],[groupid],[olimg],[adminid],[invisible],[action],[lastactivity],[lastposttime],[lastpostpmtime],[lastsearchtime],[lastupdatetime],[forumid],[forumname],[titleid],[title],[verifycode],[newpms],[newnotices] FROM [{0}online]", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, sql);
        }

        /// <summary>
        /// 返回在线用户图例
        /// </summary>
        /// <returns></returns>
        public DataTable GetOnlineGroupIconTable()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT [groupid], [displayorder], [title], [img] FROM [" + BaseConfigs.GetTablePrefix + "onlinelist] WHERE [img] <> '' ORDER BY [displayorder]").Tables[0];
        }

        /// <summary>
        /// 根据uid获得olid
        /// </summary>
        /// <param name="uid">uid</param>
        /// <returns>olid</returns>
        public int GetOlidByUid(int uid)
        {
            DbParameter[] parms = { DbHelper.MakeInParam("@userid", DbType.Int32, 4, uid) };
            return Utils.StrToInt(DbHelper.ExecuteScalarToStr(CommandType.Text, string.Format("SELECT [olid] FROM [{0}online] WHERE [userid]=@userid", BaseConfigs.GetTablePrefix), parms), -1);
        }

        /// <summary>
        /// 获得指定在线用户
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <returns>在线用户的详细信息</returns>
        public IDataReader GetOnlineUser(int olid)
        {
            DbParameter[] parms = { DbHelper.MakeInParam("@olid", DbType.Int32, 4, olid) };
            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT * FROM [{0}online] WHERE [olid]=@olid", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 获得指定用户的详细信息
        /// </summary>
        /// <param name="userid">在线用户ID</param>
        /// <param name="password">用户密码</param>
        /// <returns>用户的详细信息</returns>
        public DataTable GetOnlineUser(int userid, string password)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@userid", DbType.Int32, 4, userid),
                                        DbHelper.MakeInParam("@password", DbType.AnsiString, 32, password)
                                    };
            //return DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT TOP 1 * FROM [{0}online] WHERE [userid]=@userid AND [password]=@password", BaseConfigs.GetTablePrefix), parms).Tables[0];
            string sql = string.Format("SELECT `olid`,`userid`,`ip`,`username`,`nickname`,`password`,`groupid`,`olimg`,`adminid`,`invisible`,`action`,`lastactivity`,`lastposttime`,`lastpostpmtime`,`lastsearchtime`,`lastupdatetime`,`forumid`,`forumname`,`titleid`,`title`,`verifycode`,`newpms`,`newnotices` FROM `{0}online` WHERE `userid`=@userid AND `password`=@password", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0];
        }

        /// <summary>
        /// 获得指定用户的详细信息
        /// </summary>
        /// <param name="userid">在线用户ID</param>
        /// <param name="ip">IP</param>
        /// <returns></returns>
        public DataTable GetOnlineUserByIP(int userid, string ip)
        {
            System.Diagnostics.Debug.WriteLine("GetOnlineUserByIP(int userid, string ip)");
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@userid", DbType.Int32, 4, userid),
                                        DbHelper.MakeInParam("@ip", DbType.AnsiString, 15, ip)
                                    };
            //return DbHelper.ExecuteDataset(CommandType.StoredProcedure, string.Format("{0}getonlineuserbyip", BaseConfigs.GetTablePrefix), parms).Tables[0];
            string sql = string.Format("SELECT [olid],[userid],[ip],[username],[nickname],[password],[groupid],[olimg],[adminid],[invisible],[action],[lastactivity],[lastposttime],[lastpostpmtime],[lastsearchtime],[lastupdatetime],[forumid],[forumname],[titleid],[title],[verifycode],[newpms],[newnotices] FROM [{0}online] WHERE [userid]=@userid AND [ip]=@ip LIMIT 0,1", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0];
        }

        /// <summary>
        /// 检查在线用户验证码是否有效
        /// </summary>
        /// <param name="olid">在组用户ID</param>
        /// <param name="verifycode">验证码</param>
        /// <returns>在组用户ID</returns>
        public bool CheckUserVerifyCode(int olid, string verifycode, string newverifycode)
        {
            DbParameter[] parms = { 
                                        DbHelper.MakeInParam("@olid", DbType.Int32, 4, olid),
                                        DbHelper.MakeInParam("@verifycode", DbType.AnsiString, 10, verifycode)
                                    };
            DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, string.Format("SELECT TOP 1 [olid] FROM [{0}online] WHERE [olid]=@olid and [verifycode]=@verifycode", BaseConfigs.GetTablePrefix), parms).Tables[0];
            parms[1].Value = newverifycode;
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}online] SET [verifycode]=@verifycode WHERE [olid]=@olid", BaseConfigs.GetTablePrefix), parms);
            return dt.Rows.Count > 0;
        }

        /// <summary>
        /// 设置用户在线状态
        /// </summary>
        /// <param name="uid">用户Id</param>
        /// <param name="onlinestate">在线状态，１在线</param>
        /// <returns></returns>
        public int SetUserOnlineState(int uid, int onlinestate)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@onlinestate",DbType.Int32,4,onlinestate),
									   DbHelper.MakeInParam("@uid",DbType.Int32,4,uid)
								   };
            string sql = string.Format("UPDATE `{0}users` SET `onlinestate`=@onlinestate,`lastactivity`=datetime('now','localtime'),`lastvisit`=datetime('now','localtime') WHERE `uid`=@uid", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        /// <summary>
        /// 删除符合条件的一个或多个用户信息
        /// </summary>
        /// <returns>删除行数</returns>
        public int DeleteRowsByIP(string ip)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@ip",DbType.AnsiString,15,ip)
								   };
            string sql = string.Format("UPDATE `{0}users` SET `onlinestate`=0,`lastactivity`=datetime('now','localtime') WHERE `uid` IN (SELECT `userid` FROM `{0}online` WHERE `userid`>0 AND `ip`=@ip)", BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
            if (ip != "0.0.0.0")
            {
                string sql2 = string.Format("DELETE FROM `{0}online` WHERE `userid`=-1 AND `ip`=@ip", BaseConfigs.GetTablePrefix);
                return DbHelper.ExecuteNonQuery(CommandType.Text, sql2, parms);
            }
            return 0;
        }

        /// <summary>
        /// 删除在线表中指定在线id的行
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <returns></returns>
        public int DeleteRows(int olid)
        {
            return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "online] WHERE [olid]=" + olid.ToString());
        }

        /// <summary>
        /// 更新用户的当前动作及相关信息
        /// </summary>
        /// <param name="olid">在线列表id</param>
        /// <param name="action">动作</param>
        /// <param name="inid">所在位置代码</param>
        public void UpdateAction(int olid, int action, int inid)
        {
            System.Diagnostics.Debug.WriteLine("UpdateAction(int olid, int action, int inid)");
            DbParameter[] parms = {
										   //DbHelper.MakeInParam("@tickcount",DbType.Int32,4,System.Environment.TickCount),
										   DbHelper.MakeInParam("@action",DbType.Int16,2,action),
                                           DbHelper.MakeInParam("@lastupdatetime", DbType.DateTime, 8, DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))),
										   DbHelper.MakeInParam("@forumid",DbType.Int32,4,inid),
										   DbHelper.MakeInParam("@forumname",DbType.String,100,""),
										   DbHelper.MakeInParam("@titleid",DbType.Int32,4,inid),
										   DbHelper.MakeInParam("@title",DbType.String,160,""),
										   DbHelper.MakeInParam("@olid",DbType.Int32,4,olid)

									   };
            //DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "online] SET [lastactivity]=[action],[action]=@action,[lastupdatetime]=@lastupdatetime,[forumid]=@forumid,[forumname]=@forumname,[titleid]=@titleid,[title]=@title WHERE [olid]=@olid", parms);
            string sql = string.Format("UPDATE {0}online SET lastactivity=action,action=@action,lastupdatetime=@lastupdatetime,forumid=@forumid,forumname=@forumname,titleid=@titleid,title=@title WHERE olid=@olid", BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);

        }

        /// <summary>
        /// 更新用户的当前动作及相关信息
        /// </summary>
        /// <param name="olid">在线列表id</param>
        /// <param name="action">动作id</param>
        /// <param name="fid">版块id</param>
        /// <param name="forumname">版块名</param>
        /// <param name="tid">主题id</param>
        /// <param name="topictitle">主题名</param>
        public void UpdateAction(int olid, int action, int fid, string forumname, int tid, string topictitle)
        {
            DbParameter[] parms = {
										   //DbHelper.MakeInParam("@tickcount",DbType.Int32,4,System.Environment.TickCount),
										   DbHelper.MakeInParam("@action",DbType.Int16,2,action),
                                           DbHelper.MakeInParam("@lastupdatetime", DbType.DateTime, 8, DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))),
										   DbHelper.MakeInParam("@forumid",DbType.Int32,4,fid),
										   DbHelper.MakeInParam("@forumname",DbType.String,100,forumname),
										   DbHelper.MakeInParam("@titleid",DbType.Int32,4,tid),
										   DbHelper.MakeInParam("@title",DbType.String,160,topictitle),
										   DbHelper.MakeInParam("@olid",DbType.Int32,4,olid)

									   };
            //DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "online] SET [lastactivity]=[action],[action]=@action,[lastupdatetime]=@lastupdatetime,[forumid]=@forumid,[forumname]=@forumname,[titleid]=@titleid,[title]=@title WHERE [olid]=@olid", parms);
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "updateonlineaction", parms);
        }

        /// <summary>
        /// 更新用户最后活动时间
        /// </summary>
        /// <param name="olid">在线id</param>
        public void UpdateLastTime(int olid)
        {
            DbParameter[] parms = {
										   //DbHelper.MakeInParam("@tickcount",DbType.Int32,4,System.Environment.TickCount),
                                           DbHelper.MakeInParam("@lastupdatetime", DbType.DateTime, 8, DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))),
										   DbHelper.MakeInParam("@olid",DbType.Int32,4,olid)

									   };
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "online] SET [lastupdatetime]=@lastupdatetime WHERE [olid]=@olid", parms);
        }

        /// <summary>
        /// 更新用户最后发帖时间
        /// </summary>
        /// <param name="olid">在线id</param>
        public void UpdatePostTime(int olid)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}online] SET [lastposttime]='{1}' WHERE [olid]={2}", BaseConfigs.GetTablePrefix, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), olid.ToString()));
        }

        /// <summary>
        /// 更新用户最后发短消息时间
        /// </summary>
        /// <param name="olid">在线id</param>
        public void UpdatePostPMTime(int olid)
        {

            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}online] SET [lastpostpmtime]='{1}' WHERE [olid]={2}", BaseConfigs.GetTablePrefix, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), olid.ToString()));

        }

        /// <summary>
        /// 更新在线表中指定用户是否隐身
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <param name="invisible">是否隐身</param>
        public void UpdateInvisible(int olid, int invisible)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}online] SET [invisible]={1} WHERE [olid]={2}", BaseConfigs.GetTablePrefix, invisible.ToString(), olid.ToString()));
        }

        /// <summary>
        /// 更新在线表中指定用户的用户密码
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <param name="password">用户密码</param>
        public void UpdatePassword(int olid, string password)
        {

            DbParameter[] parms = {
									   DbHelper.MakeInParam("@password",DbType.AnsiString,32,password),
									   DbHelper.MakeInParam("@olid",DbType.Int32,4,olid)

								   };
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}online] SET [password]=@password WHERE [olid]=@olid", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 更新用户IP地址
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <param name="ip">ip地址</param>
        public void UpdateIP(int olid, string ip)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@ip",DbType.AnsiString,15,ip),
									   DbHelper.MakeInParam("@olid",DbType.Int32,4,olid)

								   };
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}online] SET [ip]=@ip WHERE [olid]=@olid", BaseConfigs.GetTablePrefix), parms);

        }

        /// <summary>
        /// 更新用户最后搜索时间
        /// </summary>
        /// <param name="olid">在线id</param>
        public void UpdateSearchTime(int olid)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}online] SET [lastsearchtime]={1} WHERE [olid]={2}", BaseConfigs.GetTablePrefix, olid.ToString()));
        }

        /// <summary>
        /// 更新用户的用户组
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="groupid">组名</param>
        public void UpdateGroupid(int userid, int groupid)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}online] SET [groupid]={1} WHERE [userid]={2}", BaseConfigs.GetTablePrefix, groupid.ToString(), userid.ToString()));
        }


        /// <summary>
        /// 获得指定ID的短消息的内容
        /// </summary>
        /// <param name="pmid">短消息pmid</param>
        /// <returns>短消息内容</returns>
        public IDataReader GetPrivateMessageInfo(int pmid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@pmid", DbType.Int32,4, pmid),
			                        };
            string sql = string.Format("SELECT * FROM `{0}pms` WHERE `pmid`=@pmid LIMIT 0,1", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, sql, parms);
        }

        /// <summary>
        /// 获得指定用户的短信息列表
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="folder">短信息类型(0:收件箱,1:发件箱,2:草稿箱)</param>
        /// <param name="pagesize">每页显示短信息数</param>
        /// <param name="pageindex">当前要显示的页数</param>
        /// <param name="inttypewhere">筛选条件1为未读</param>
        /// <returns>短信息列表</returns>
        public IDataReader GetPrivateMessageList(int userid, int folder, int pagesize, int pageindex, int inttype)
        {
            string strwhere = "";
            if (inttype == 1)
            {
                strwhere = "[new]=1";
            }
            int recordoffset = (pageindex - 1) * pagesize;
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@userid",DbType.Int32,4,userid),
									   DbHelper.MakeInParam("@folder",DbType.Int32,4,folder),
									   DbHelper.MakeInParam("@pagesize", DbType.Int32,4,pagesize),
									   DbHelper.MakeInParam("@recordoffset",DbType.Int32,4,recordoffset)									   
								   };
            string sql;
            string msgformORtoID = "msgtoid";
            if (folder == 1 || folder == 2)
            {
                msgformORtoID = "msgfromid";
            }
            if (strwhere != string.Empty)
            {
                sql = string.Format("SELECT `pmid`,`msgfrom`,`msgfromid`,`msgto`,`msgtoid`,`folder`,`new`,`subject`,`postdatetime`,`message` FROM `{0}pms` WHERE `{1}`=@userid AND `folder`=@folder AND {2} ORDER BY `pmid` DESC LIMIT @recordoffset,@pagesize", BaseConfigs.GetTablePrefix, msgformORtoID, strwhere);
            }
            else
            {
                sql = string.Format("SELECT `pmid`,`msgfrom`,`msgfromid`,`msgto`,`msgtoid`,`folder`,`new`,`subject`,`postdatetime`,`message` FROM `{0}pms` WHERE `{1}`=@userid AND `folder`=@folder ORDER BY `pmid` DESC LIMIT @recordoffset,@pagesize", BaseConfigs.GetTablePrefix, msgformORtoID);
            }
            IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, sql, parms);
            return reader;
        }

        /// <summary>
        /// 得到当用户的短消息数量
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="folder">所属文件夹(0:收件箱,1:发件箱,2:草稿箱)</param>
        /// <param name="state">短消息状态(0:已读短消息、1:未读短消息、-1:全部短消息)</param>
        /// <returns>短消息数量</returns>
        public int GetPrivateMessageCount(int userid, int folder, int state)
        {
            string sql;
            if (folder == 1)
            {
                sql = string.Format("SELECT COUNT(pmid) AS `pmcount` FROM `{0}pms` WHERE (`msgtoid`=@userid AND `folder`=0) OR (`msgfromid` = @userid AND `folder` = 1) OR (`msgfromid` = @userid AND `folder` = 2)", BaseConfigs.GetTablePrefix);
            }
            else if (folder == 0)
            {
                if (state == -1)
                {
                    sql = string.Format("SELECT COUNT(pmid) AS `pmcount` FROM `{0}pms` WHERE `msgtoid`=@userid AND `folder`=@folder", BaseConfigs.GetTablePrefix);
                }
                else if (state == 2)
                {
                    sql = string.Format("SELECT COUNT(pmid) AS `pmcount` FROM `{0}pms` WHERE `msgtoid`=@userid AND `folder`=@folder AND `new`=1 AND (julianday(datetime('now','localtime'))-`postdatetime`)", BaseConfigs.GetTablePrefix);
                }
                else
                {
                    sql = string.Format("SELECT COUNT(pmid) AS `pmcount` FROM `{0}pms` WHERE `msgtoid`=@userid AND `folder`=@folder AND `new`=@state", BaseConfigs.GetTablePrefix);
                }
            }
            else
            {
                if (state == -1)
                {
                    sql = string.Format("SELECT COUNT(pmid) AS `pmcount` FROM `{0}pms` WHERE `msgfromid`=@userid AND `folder`=@folder", BaseConfigs.GetTablePrefix);
                }
                else if (state == 2)
                {
                    sql = string.Format("SELECT COUNT(pmid) AS `pmcount` FROM `{0}pms` WHERE `msgfromid`=@userid AND `folder`=@folder AND `new`=1 AND (julianday(datetime('now','localtime'))-`postdatetime`)<3", BaseConfigs.GetTablePrefix);
                }
                else
                {
                    sql = string.Format("SELECT COUNT(pmid) AS `pmcount` FROM `{0}pms` WHERE `msgfromid`=@userid AND `folder`=@folder AND `new`=@state", BaseConfigs.GetTablePrefix);
                }
            }
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@userid",DbType.Int32,4,userid),
									   DbHelper.MakeInParam("@folder",DbType.Int32,4,folder),								   
									   DbHelper.MakeInParam("@state",DbType.Int32,4,state)
								   };
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, parms).ToString(), 0);
        }

        /// <summary>
        /// 得到公共消息数量
        /// </summary>
        /// <returns>公共消息数量</returns>
        public int GetAnnouncePrivateMessageCount()
        {
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(pmid) FROM [" + BaseConfigs.GetTablePrefix + "pms] WHERE [msgtoid] = 0").ToString(), 0);
        }


        /// <summary>
        /// 获得指定用户的短信息列表
        /// </summary>
        /// <param name="pagesize">每页显示短信息数</param>
        /// <param name="pageindex">当前要显示的页数</param>
        /// <returns>短信息列表</returns>
        public IDataReader GetAnnouncePrivateMessageList(int pagesize, int pageindex)
        {
            string sql = "";
            if(pageindex <= 1)
            {
	   		    sql = string.Format("SELECT TOP {0} * FROM [{1}pms] WHERE [msgtoid] = 0  ORDER BY [pmid] DESC", pagesize, BaseConfigs.GetTablePrefix);
            }
	        else
            {
                sql = string.Format("SELECT TOP {0} * FROM [{1}pms] WHERE [msgtoid] = 0 AND [pmid] < (SELECT MIN([pmid]) FROM (SELECT TOP " + (pageindex - 1) * pagesize + " [pmid] FROM [{1}pms] WHERE [msgtoid] = 0  ORDER BY [pmid] DESC) AS tblTmp)  ORDER BY [pmid] DESC", pagesize, BaseConfigs.GetTablePrefix);
            }

            IDataReader reader = DbHelper.ExecuteReader(CommandType.Text, sql);
            return reader;
        }

        /// <summary>
        /// 创建短消息
        /// </summary>
        /// <param name="__privatemessageinfo">短消息内容</param>
        /// <param name="savetosentbox">设置短消息是否在发件箱保留(0为不保留, 1为保留)</param>
        /// <returns>短消息在数据库中的pmid</returns>
        public int CreatePrivateMessage(PrivateMessageInfo pminfo, int savetosentbox)
        {
            System.Diagnostics.Debug.WriteLine("CreatePrivateMessage(PrivateMessageInfo pminfo, int savetosentbox)");
            if (pminfo.Folder != 0)
            {
                pminfo.Msgfrom = pminfo.Msgto;
            }
            else
            {
                DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE `{0}users` SET `newpmcount`=`newpmcount`+1,`newpm`=1 WHERE `uid`={1}", BaseConfigs.GetTablePrefix, pminfo.Msgtoid));
            }

            DbParameter[] parms = {
									   DbHelper.MakeInParam("@pmid",DbType.Int32,4,pminfo.Pmid),
									   DbHelper.MakeInParam("@msgfrom",DbType.String,20,pminfo.Msgfrom),
									   DbHelper.MakeInParam("@msgfromid",DbType.Int32,4,pminfo.Msgfromid),
									   DbHelper.MakeInParam("@msgto",DbType.String,20,pminfo.Msgto),
									   DbHelper.MakeInParam("@msgtoid",DbType.Int32,4,pminfo.Msgtoid),
									   DbHelper.MakeInParam("@folder",DbType.Int16,2,pminfo.Folder),
									   DbHelper.MakeInParam("@new",DbType.Int32,4,pminfo.New),
									   DbHelper.MakeInParam("@subject",DbType.String,80,pminfo.Subject),
									   DbHelper.MakeInParam("@postdatetime",DbType.DateTime,8,DateTime.Parse(pminfo.Postdatetime)),
									   DbHelper.MakeInParam("@message",DbType.String,0,pminfo.Message)
								   };
            string sql = string.Format("INSERT INTO `{0}pms` (`msgfrom`,`msgfromid`,`msgto`,`msgtoid`,`folder`,`new`,`subject`,`postdatetime`,`message`) VALUES	(@msgfrom,@msgfromid,@msgto,@msgtoid,@folder,@new,@subject,@postdatetime,@message);select last_insert_rowid() AS pmid", BaseConfigs.GetTablePrefix);
            int pmid = Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, parms).ToString(), -1);

            if (savetosentbox == 1 && pminfo.Folder == 0)
            {
                DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("INSERT INTO `{0}pms` (`msgfrom`,`msgfromid`,`msgto`,`msgtoid`,`folder`,`new`,`subject`,`postdatetime`,`message`) VALUES(@msgfrom,@msgfromid,@msgto,@msgtoid,1,@new,@subject,@postdatetime,@message)", BaseConfigs.GetTablePrefix), parms);
            }
            return pmid;
        }

        /// <summary>
        /// 删除指定用户的短信息
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="pmitemid">要删除的短信息列表(数组)</param>
        /// <returns>删除记录数</returns>
        public int DeletePrivateMessages(int userid, string pmidlist)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@userid", DbType.Int32,4, userid)
			};

            return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "pms] WHERE [pmid] IN (" + pmidlist + ") AND ([msgtoid] = @userid OR [msgfromid] = @userid)", parms);

        }

        /// <summary>
        /// 获得新短消息数
        /// </summary>
        /// <returns></returns>
        public int GetNewPMCount(int userid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@userid", DbType.Int32,4, userid)
			};
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT([pmid]) AS [pmcount] FROM [" + BaseConfigs.GetTablePrefix + "pms] WHERE [new] = 1 AND [folder] = 0 AND [msgtoid] = @userid", parms), 0);
        }

        /// <summary>
        /// 删除指定用户的一条短消息
        /// </summary>
        /// <param name="userid">用户Ｉｄ</param>
        /// <param name="pmid">ＰＭＩＤ</param>
        /// <returns></returns>
        public int DeletePrivateMessage(int userid, int pmid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@userid", DbType.Int32,4, userid),
									   DbHelper.MakeInParam("@pmid", DbType.Int32,4, pmid)
			};
            return DbHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM [" + BaseConfigs.GetTablePrefix + "pms] WHERE [pmid]=@pmid AND ([msgtoid] = @userid OR [msgfromid] = @userid)", parms);

        }

        /// <summary>
        /// 设置短信息状态
        /// </summary>
        /// <param name="pmid">短信息ID</param>
        /// <param name="state">状态值</param>
        /// <returns>更新记录数</returns>
        public int SetPrivateMessageState(int pmid, byte state)
        {

            DbParameter[] parms = {
									   DbHelper.MakeInParam("@pmid", DbType.Int32,1,pmid),
									   DbHelper.MakeInParam("@state",DbType.Byte,1,state)
								   };
            return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "pms] SET [new]=@state WHERE [pmid]=@pmid", parms);

        }

        public int GetRAdminIdByGroup(int groupid)
        {
            return Convert.ToInt32(DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [radminid] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]=" + groupid).Tables[0].Rows[0][0].ToString());
        }

        public string GetUserGroupsStr()
        {
            return "SELECT [groupid], [grouptitle] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] ORDER BY [groupid]";
        }


        public DataTable GetUserNameListByGroupid(string groupidlist)
        {
            string sql = "SELECT [uid] ,[username]  From [" + BaseConfigs.GetTablePrefix + "users] WHERE [groupid] IN(" + groupidlist + ")";
            return DbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public DataTable GetUserNameByUid(int uid)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@uid", DbType.Int32, 4, uid)
			};
            string sql = "SELECT TOP 1 [username] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid]=@uid";
            return DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0];
        }

        public void ResetPasswordUid(string password, int uid)
        {
            DbParameter[] parms = 
			{
                DbHelper.MakeInParam("@password", DbType.AnsiString, 32, password),
				DbHelper.MakeInParam("@uid", DbType.Int32, 4, uid)
			};
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [password]=@password WHERE [uid]=@uid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public void SendPMToUser(string msgfrom, int msgfromid, string msgto, int msgtoid, int folder, string subject, DateTime postdatetime, string message)
        {
            DbParameter[] parms = 
			{
				DbHelper.MakeInParam("@msgfrom", DbType.String,50, msgfrom),
				DbHelper.MakeInParam("@msgfromid", DbType.Int32, 4, msgfromid),
				DbHelper.MakeInParam("@msgto", DbType.String,50, msgto),
				DbHelper.MakeInParam("@msgtoid", DbType.Int32, 4, msgtoid),
                DbHelper.MakeInParam("@folder", DbType.Int16, 2, folder),
                DbHelper.MakeInParam("@subject", DbType.String,60, subject),
                DbHelper.MakeInParam("@postdatetime", DbType.DateTime,8, postdatetime),
				DbHelper.MakeInParam("@message",DbType.String, 0,message)
			};
            string sql = "INSERT INTO [" + BaseConfigs.GetTablePrefix + "pms] (msgfrom,msgfromid,msgto,msgtoid,folder,new,subject,postdatetime,message) " +
                "VALUES (@msgfrom,@msgfromid,@msgto,@msgtoid,@folder,1,@subject,@postdatetime,@message)";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
            sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [newpmcount]=[newpmcount]+1  WHERE [uid] =@msgtoid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public string GetSystemGroupInfoSql()
        {
            return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]<=8 ORDER BY [groupid]";
        }

        public void UpdateUserCredits(int uid, string credits)
        {
            DbParameter[] prams_credits = {
											   DbHelper.MakeInParam("@uid", DbType.Int32, 4, uid)
										   };

            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}users] SET [credits] = {1} WHERE [uid]=@uid", BaseConfigs.GetTablePrefix, credits), prams_credits);
        }

        public void UpdateUserGroup(int uid, int groupid)
        {
            DbParameter[] prams_credits = {
											   DbHelper.MakeInParam("@uid", DbType.Int32, 4, uid)
										   };

            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}users] SET [groupid] = {1} WHERE [uid]=@uid", BaseConfigs.GetTablePrefix, groupid), prams_credits);

        }

        public bool CheckUserCreditsIsEnough(int uid, float[] values)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid", DbType.Int32, 4, uid), 
									   DbHelper.MakeInParam("@extcredits1", DbType.Single, 8, values[0]),
									   DbHelper.MakeInParam("@extcredits2", DbType.Single, 8, values[1]),
									   DbHelper.MakeInParam("@extcredits3", DbType.Single, 8, values[2]),
									   DbHelper.MakeInParam("@extcredits4", DbType.Single, 8, values[3]),
									   DbHelper.MakeInParam("@extcredits5", DbType.Single, 8, values[4]),
									   DbHelper.MakeInParam("@extcredits6", DbType.Single, 8, values[5]),
									   DbHelper.MakeInParam("@extcredits7", DbType.Single, 8, values[6]),
									   DbHelper.MakeInParam("@extcredits8", DbType.Single, 8, values[7])
								   };
            string CommandText = "SELECT COUNT(1) FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid]=@uid AND"
                    + "	[extcredits1]>= (case when @extcredits1<0 then abs(@extcredits1) else [extcredits1] end) AND "
                    + "	[extcredits2]>= (case when @extcredits2<0 then abs(@extcredits2) else [extcredits2] end) AND "
                    + "	[extcredits3]>= (case when @extcredits3<0 then abs(@extcredits3) else [extcredits3] end) AND "
                    + "	[extcredits4]>= (case when @extcredits4<0 then abs(@extcredits4) else [extcredits4] end) AND "
                    + "	[extcredits5]>= (case when @extcredits5<0 then abs(@extcredits5) else [extcredits5] end) AND "
                    + "	[extcredits6]>= (case when @extcredits6<0 then abs(@extcredits6) else [extcredits6] end) AND "
                    + "	[extcredits7]>= (case when @extcredits7<0 then abs(@extcredits7) else [extcredits7] end) AND "
                    + "	[extcredits8]>= (case when @extcredits8<0 then abs(@extcredits8) else [extcredits8] end) ";

            if (Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, CommandText, parms)) == 0)
            {
                return false;
            }
            return true;
        }

        public void UpdateUserCredits(int uid, float[] values)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid", DbType.Int32, 4, uid), 
									   DbHelper.MakeInParam("@extcredits1", DbType.Single, 8, values[0]),
									   DbHelper.MakeInParam("@extcredits2", DbType.Single, 8, values[1]),
									   DbHelper.MakeInParam("@extcredits3", DbType.Single, 8, values[2]),
									   DbHelper.MakeInParam("@extcredits4", DbType.Single, 8, values[3]),
									   DbHelper.MakeInParam("@extcredits5", DbType.Single, 8, values[4]),
									   DbHelper.MakeInParam("@extcredits6", DbType.Single, 8, values[5]),
									   DbHelper.MakeInParam("@extcredits7", DbType.Single, 8, values[6]),
									   DbHelper.MakeInParam("@extcredits8", DbType.Single, 8, values[7])
								   };

            string CommandText = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET "
                + "		[extcredits1]=[extcredits1] + @extcredits1, "
                + "		[extcredits2]=[extcredits2] + @extcredits2, "
                + "		[extcredits3]=[extcredits3] + @extcredits3, "
                + "		[extcredits4]=[extcredits4] + @extcredits4, "
                + "		[extcredits5]=[extcredits5] + @extcredits5, "
                + "		[extcredits6]=[extcredits6] + @extcredits6, "
                + "		[extcredits7]=[extcredits7] + @extcredits7, "
                + "		[extcredits8]=[extcredits8] + @extcredits8 "
                + "WHERE [uid]=@uid";

            DbHelper.ExecuteNonQuery(CommandType.Text, CommandText, parms);
        }

        public bool CheckUserCreditsIsEnough(int uid, DataRow values, int pos, int mount)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid", DbType.Int32, 4, uid), 
									   DbHelper.MakeInParam("@extcredits1", DbType.Single, 8, Utils.StrToFloat(values["extcredits1"],0) * pos * mount),
									   DbHelper.MakeInParam("@extcredits2", DbType.Single, 8, Utils.StrToFloat(values["extcredits2"],0) * pos * mount),
									   DbHelper.MakeInParam("@extcredits3", DbType.Single, 8, Utils.StrToFloat(values["extcredits3"],0) * pos * mount),
									   DbHelper.MakeInParam("@extcredits4", DbType.Single, 8, Utils.StrToFloat(values["extcredits4"],0) * pos * mount),
									   DbHelper.MakeInParam("@extcredits5", DbType.Single, 8, Utils.StrToFloat(values["extcredits5"],0) * pos * mount),
									   DbHelper.MakeInParam("@extcredits6", DbType.Single, 8, Utils.StrToFloat(values["extcredits6"],0) * pos * mount),
									   DbHelper.MakeInParam("@extcredits7", DbType.Single, 8, Utils.StrToFloat(values["extcredits7"],0) * pos * mount),
									   DbHelper.MakeInParam("@extcredits8", DbType.Single, 8, Utils.StrToFloat(values["extcredits8"],0) * pos * mount)
								   };
            string CommandText = "SELECT COUNT(1) FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid]=@uid AND"
                    + "	[extcredits1]>= (case when @extcredits1 >= 0 then abs(@extcredits1) else 0 end) AND "
                    + "	[extcredits2]>= (case when @extcredits2 >= 0 then abs(@extcredits2) else 0 end) AND "
                    + "	[extcredits3]>= (case when @extcredits3 >= 0 then abs(@extcredits3) else 0 end) AND "
                    + "	[extcredits4]>= (case when @extcredits4 >= 0 then abs(@extcredits4) else 0 end) AND "
                    + "	[extcredits5]>= (case when @extcredits5 >= 0 then abs(@extcredits5) else 0 end) AND "
                    + "	[extcredits6]>= (case when @extcredits6 >= 0 then abs(@extcredits6) else 0 end) AND "
                    + "	[extcredits7]>= (case when @extcredits7 >= 0 then abs(@extcredits7) else 0 end) AND "
                    + "	[extcredits8]>= (case when @extcredits8 >= 0 then abs(@extcredits8) else 0 end) ";

            if (Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, CommandText, parms)) == 0)
            {
                return false;
            }
            return true;
        }

        public void UpdateUserCredits(int uid, DataRow values, int pos, int mount)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid", DbType.Int32, 4, uid), 
									   DbHelper.MakeInParam("@extcredits1", DbType.Single, 8, Utils.StrToFloat(values["extcredits1"],0) * pos * mount),
									   DbHelper.MakeInParam("@extcredits2", DbType.Single, 8, Utils.StrToFloat(values["extcredits2"],0) * pos * mount),
									   DbHelper.MakeInParam("@extcredits3", DbType.Single, 8, Utils.StrToFloat(values["extcredits3"],0) * pos * mount),
									   DbHelper.MakeInParam("@extcredits4", DbType.Single, 8, Utils.StrToFloat(values["extcredits4"],0) * pos * mount),
									   DbHelper.MakeInParam("@extcredits5", DbType.Single, 8, Utils.StrToFloat(values["extcredits5"],0) * pos * mount),
									   DbHelper.MakeInParam("@extcredits6", DbType.Single, 8, Utils.StrToFloat(values["extcredits6"],0) * pos * mount),
									   DbHelper.MakeInParam("@extcredits7", DbType.Single, 8, Utils.StrToFloat(values["extcredits7"],0) * pos * mount),
									   DbHelper.MakeInParam("@extcredits8", DbType.Single, 8, Utils.StrToFloat(values["extcredits8"],0) * pos * mount)
								   };
            if (pos < 0 && mount < 0)
            {
                for (int i = 1; i < parms.Length; i++)
                {
                    parms[i].Value = -Convert.ToInt32(parms[i].Value);
                }
            }

            string CommandText = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET "
                + "	[extcredits1]=[extcredits1] + @extcredits1, "
                + "	[extcredits2]=[extcredits2] + @extcredits2, "
                + "	[extcredits3]=[extcredits3] + @extcredits3, "
                + "	[extcredits4]=[extcredits4] + @extcredits4, "
                + "	[extcredits5]=[extcredits5] + @extcredits5, "
                + "	[extcredits6]=[extcredits6] + @extcredits6, "
                + "	[extcredits7]=[extcredits7] + @extcredits7, "
                + "	[extcredits8]=[extcredits8] + @extcredits8 "
                + "WHERE [uid]=@uid";

            DbHelper.ExecuteNonQuery(CommandType.Text, CommandText, parms);
        }


        public DataTable GetUserGroups()
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "usergroups] ORDER BY [groupid]").Tables[0];
        }

        public DataTable GetUserGroupRateRange(int groupid)
        {
            return DbHelper.ExecuteDataset(CommandType.Text, "SELECT TOP 1 [raterange] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]=" + groupid.ToString()).Tables[0];
        }

        public IDataReader GetUserTodayRate(int uid)
        {
            DbParameter[] parms = {
						                DbHelper.MakeInParam("@uid", DbType.Int32, 4, uid)
								    };
            return DbHelper.ExecuteReader(CommandType.Text, "SELECT [extcredits], SUM(ABS([score])) AS [todayrate] FROM [" + BaseConfigs.GetTablePrefix + "ratelog] WHERE DATEDIFF(d,[postdatetime],getdate()) = 0 AND [uid] = @uid GROUP BY [extcredits]", parms);
        }


        public string GetSpecialGroupInfoSql()
        {
            return "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [radminid]=-1 AND [groupid]>8 ORDER BY [groupid]";
        }


        ///// <summary>
        ///// 更新在线时间
        ///// </summary>
        ///// <param name="uid">用户id</param>
        ///// <returns></returns>
        //public int UpdateOnlineTime(int uid)
        //{
        //    return DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}users] SET [oltime] = [oltime] + DATEDIFF(n,[lastvisit],GETDATE()) WHERE [uid]={1}", BaseConfigs.GetTablePrefix, uid));
        //}


        /// <summary>
        /// 返回指定用户的信息
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>用户信息</returns>
        public IDataReader GetUserInfoToReader(int uid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid", DbType.Int32,4, uid)
			};
            string sql = string.Format("SELECT `{0}users`.*, `{0}userfields`.* FROM `{0}users` LEFT JOIN `{0}userfields` ON `{0}users`.`uid`=`{0}userfields`.`uid` WHERE `{0}users`.`uid`=@uid LIMIT 0,1", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, sql, parms);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public IDataReader GetUserInfoToReader(string uname)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uname", DbType.AnsiString,20, uname)
			};

            return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 [" + BaseConfigs.GetTablePrefix + "_users].*, [" + BaseConfigs.GetTablePrefix + "_userfields].* FROM [" + BaseConfigs.GetTablePrefix + "_users] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "_userfields] ON [" + BaseConfigs.GetTablePrefix + "_users].[uid]=[" + BaseConfigs.GetTablePrefix + "_userfields].[uid] WHERE [" + BaseConfigs.GetTablePrefix + "_users].[username]=@uname", parms);
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public IDataReader GetUserInfoToReader(int uid, string uname)
        {
            DbParameter[] parms = {
                                       DbHelper.MakeInParam("@uid",DbType.Int32,4, uid),
									   DbHelper.MakeInParam("@uname", DbType.AnsiString,20, uname)
			};

            return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 [" + BaseConfigs.GetTablePrefix + "_users].*, [" + BaseConfigs.GetTablePrefix + "_userfields].* FROM [" + BaseConfigs.GetTablePrefix + "_users] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "_userfields] ON [" + BaseConfigs.GetTablePrefix + "_users].[uid]=[" + BaseConfigs.GetTablePrefix + "_userfields].[uid] WHERE [" + BaseConfigs.GetTablePrefix + "_users].[uid]=@uid AND[" + BaseConfigs.GetTablePrefix + "_users].[username]=@uname", parms);
        
        }
        /// <summary>
        /// 获取简短用户信息
        /// </summary>
        /// <param name="uid">用id</param>
        /// <returns>用户简短信息</returns>
        public IDataReader GetShortUserInfoToReader(int uid)
        {

            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid", DbType.Int32,4, uid),
			};
            string sql = string.Format("SELECT * FROM `{0}users` WHERE `uid`=@uid LIMIT 0,1", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, sql, parms);

        }

        /// <summary>
        /// 根据IP查找用户
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <returns>用户信息</returns>
        public IDataReader GetUserInfoByIP(string ip)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@regip", DbType.AnsiString,15, ip),
			};

            return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 [" + BaseConfigs.GetTablePrefix + "users].*, [" + BaseConfigs.GetTablePrefix + "userfields].* FROM [" + BaseConfigs.GetTablePrefix + "users] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "userfields] ON [" + BaseConfigs.GetTablePrefix + "users].[uid]=[" + BaseConfigs.GetTablePrefix + "userfields].[uid] WHERE [" + BaseConfigs.GetTablePrefix + "users].[regip]=@regip ORDER BY [" + BaseConfigs.GetTablePrefix + "users].[uid] DESC", parms);

        }

        public IDataReader GetUserName(int uid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid",DbType.Int32,4,uid),
			};
            return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 [username] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [" + BaseConfigs.GetTablePrefix + "users].[uid]=@uid", parms);

        }

        public IDataReader GetUserJoinDate(int uid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid",DbType.Int32,4,uid),
			};
            return DbHelper.ExecuteReader(CommandType.Text, "SELECT [joindate] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [" + BaseConfigs.GetTablePrefix + "users].[uid]=@uid LIMIT 0,1", parms);
        }

        public IDataReader GetUserID(string username)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@username",DbType.AnsiString,20,username),
			};
            return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 [uid] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [" + BaseConfigs.GetTablePrefix + "users].[username]=@username", parms);
        }

        public DataTable GetUserList(int pagesize, int currentpage)
        {
            #region 获得用户列表
            int pagetop = (currentpage - 1) * pagesize;

            if (currentpage == 1)
            {
                return DbHelper.ExecuteDataset("SELECT TOP " + pagesize.ToString() + " [" + BaseConfigs.GetTablePrefix + "users].[uid], [" + BaseConfigs.GetTablePrefix + "users].[username],[" + BaseConfigs.GetTablePrefix + "users].[nickname], [" + BaseConfigs.GetTablePrefix + "users].[joindate], [" + BaseConfigs.GetTablePrefix + "users].[credits], [" + BaseConfigs.GetTablePrefix + "users].[posts], [" + BaseConfigs.GetTablePrefix + "users].[lastactivity], [" + BaseConfigs.GetTablePrefix + "users].[email],[" + BaseConfigs.GetTablePrefix + "users].[lastvisit],[" + BaseConfigs.GetTablePrefix + "users].[lastvisit],[" + BaseConfigs.GetTablePrefix + "users].[accessmasks], [" + BaseConfigs.GetTablePrefix + "userfields].[location],[" + BaseConfigs.GetTablePrefix + "usergroups].[grouptitle] FROM [" + BaseConfigs.GetTablePrefix + "users] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "userfields] ON [" + BaseConfigs.GetTablePrefix + "userfields].[uid] = [" + BaseConfigs.GetTablePrefix + "users].[uid] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "usergroups] ON [" + BaseConfigs.GetTablePrefix + "usergroups].[groupid]=[" + BaseConfigs.GetTablePrefix + "users].[groupid] ORDER BY [" + BaseConfigs.GetTablePrefix + "users].[uid] DESC").Tables[0];
            }
            else
            {
                string sqlstring = "SELECT TOP " + pagesize.ToString() + " [" + BaseConfigs.GetTablePrefix + "users].[uid], [" + BaseConfigs.GetTablePrefix + "users].[username],[" + BaseConfigs.GetTablePrefix + "users].[nickname], [" + BaseConfigs.GetTablePrefix + "users].[joindate], [" + BaseConfigs.GetTablePrefix + "users].[credits], [" + BaseConfigs.GetTablePrefix + "users].[posts], [" + BaseConfigs.GetTablePrefix + "users].[lastactivity], [" + BaseConfigs.GetTablePrefix + "users].[email],[" + BaseConfigs.GetTablePrefix + "users].[lastvisit],[" + BaseConfigs.GetTablePrefix + "users].[lastvisit],[" + BaseConfigs.GetTablePrefix + "users].[accessmasks], [" + BaseConfigs.GetTablePrefix + "userfields].[location],[" + BaseConfigs.GetTablePrefix + "usergroups].[grouptitle] FROM [" + BaseConfigs.GetTablePrefix + "users],[" + BaseConfigs.GetTablePrefix + "userfields],[" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [" + BaseConfigs.GetTablePrefix + "userfields].[uid] = [" + BaseConfigs.GetTablePrefix + "users].[uid] AND  [" + BaseConfigs.GetTablePrefix + "usergroups].[groupid]=[" + BaseConfigs.GetTablePrefix + "users].[groupid] AND [" + BaseConfigs.GetTablePrefix + "users].[uid] < (SELECT min([uid])  FROM (SELECT TOP " + pagetop + " [uid] FROM [" + BaseConfigs.GetTablePrefix + "users] ORDER BY [uid] DESC) AS tblTmp )  ORDER BY [" + BaseConfigs.GetTablePrefix + "users].[uid] DESC";
                return DbHelper.ExecuteDataset(sqlstring).Tables[0];
            }

            #endregion
        }

        /// <summary>
        /// 获得用户列表DataTable
        /// </summary>
        /// <param name="pagesize">每页记录数</param>
        /// <param name="pageindex">当前页数</param>
        /// <returns>用户列表DataTable</returns>
        public DataTable GetUserList(int pagesize, int pageindex, string orderby, string ordertype)
        {
            System.Diagnostics.Debug.WriteLine("GetUserList(int pagesize, int pageindex, string orderby, string ordertype)---有意思");
            #region 处理排序
            string[] arrayorderby = new string[] { "username", "credits", "posts", "admin", "lastactivity", "joindate", "oltime" };
            int i = Array.IndexOf(arrayorderby, orderby);

            switch (i)
            {
                //case 0:
                //    orderby = "ORDER BY [" + BaseConfigs.GetTablePrefix + "users].[uid] " + ordertype;
                //    break;
                case 0:
                    orderby = string.Format("ORDER BY [{0}users].[username] {1},[{0}users].[uid] {1}", BaseConfigs.GetTablePrefix, ordertype);
                    break;
                case 1:
                    orderby = string.Format("ORDER BY [{0}users].[credits] {1},[{0}users].[uid] {1}", BaseConfigs.GetTablePrefix, ordertype);
                    break;
                case 2:
                    orderby = string.Format("ORDER BY [{0}users].[posts] {1},[{0}users].[uid] {1}", BaseConfigs.GetTablePrefix, ordertype);
                    break;
                case 3:
                    orderby = string.Format("WHERE [{0}users].[adminid] > 0 ORDER BY [{0}users].[adminid] {1}, [{0}users].[uid] {1}", BaseConfigs.GetTablePrefix, ordertype);
                    break;
                //case "joindate":
                //    orderby = "ORDER BY [" + BaseConfigs.GetTablePrefix + "users].[joindate] " + ordertype + ",[" + BaseConfigs.GetTablePrefix + "users].[uid] " + ordertype;
                //    break;
                case 4:
                    orderby = string.Format("ORDER BY [{0}users].[lastactivity] {1},[{0}users].[uid] {1}", BaseConfigs.GetTablePrefix, ordertype);
                    break;
                case 5:
                    orderby = string.Format("ORDER BY [{0}users].[joindate] {1},[{0}users].[uid] {1}", BaseConfigs.GetTablePrefix, ordertype);
                    break;
                case 6:
                    orderby = string.Format("ORDER BY [{0}users].[oltime] {1},[{0}users].[uid] {1}", BaseConfigs.GetTablePrefix, ordertype);
                    break;
                default:
                    orderby = string.Format("ORDER BY [{0}users].[uid] {1}", BaseConfigs.GetTablePrefix, ordertype);
                    break;
            }
            #endregion
            int recordoffset = (pageindex - 1) * pagesize;
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@pagesize", DbType.Int32,4,pagesize),
									   DbHelper.MakeInParam("@recordoffset",DbType.Int32,4,recordoffset)
								   };
            string sql = string.Format("SELECT `{0}users`.`uid`,`{0}users`.`groupid`,`{0}users`.`username`,`{0}users`.`nickname`,`{0}users`.`joindate`,`{0}users`.`credits`,`{0}users`.`posts`,`{0}users`.`lastactivity`,`{0}users`.`email`,`{0}users`.`oltime`,`{0}userfields`.`location` FROM `{0}users` LEFT JOIN `{0}userfields` ON `{0}userfields`.`uid`=`{0}users`.`uid` {1} LIMIT @recordoffset,@pagesize", BaseConfigs.GetTablePrefix, orderby);
            return DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0];
        }

        /// <summary>
        /// 判断指定用户名是否已存在
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>如果已存在该用户id则返回true, 否则返回false</returns>
        public bool Exists(int uid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid",DbType.Int32,4,uid),
			};
            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(1) FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid]=@uid", parms)) >= 1;
        }

        /// <summary>
        /// 判断指定用户名是否已存在.
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>如果已存在该用户名则返回true, 否则返回false</returns>
        public bool Exists(string username)
        {
            System.Diagnostics.Debug.WriteLine("Exists(string username)");
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@username",DbType.String,20,username),
			};
            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(1) FROM {0}users WHERE `username`=@username", BaseConfigs.GetTablePrefix), parms)) >= 1;
        }

        /// <summary>
        /// 是否有指定ip地址的用户注册
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <returns>存在返回true,否则返回false</returns>
        public bool ExistsByIP(string ip)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@regip",DbType.AnsiString, 15,ip),
			};
            return Convert.ToInt32(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT COUNT(1) FROM [{0}users] WHERE [regip]=@regip", BaseConfigs.GetTablePrefix), parms)) >= 1;
        }

        /// <summary>
        /// 检测Email和安全项
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="email">email</param>
        /// <param name="questionid">问题id</param>
        /// <param name="answer">答案</param>
        /// <returns>如果正确则返回用户id, 否则返回-1</returns>
        public IDataReader CheckEmailAndSecques(string username, string email, string secques)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@username",DbType.AnsiString,20,username),
									   DbHelper.MakeInParam("@email",DbType.AnsiString,50, email),
									   DbHelper.MakeInParam("@secques",DbType.AnsiString,8, secques)
								   };
            return DbHelper.ExecuteReader(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "checkemailandsecques", parms);
        }

        /// <summary>
        /// 检测密码和安全项
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="originalpassword">是否非MD5密码</param>
        /// <param name="questionid">问题id</param>
        /// <param name="answer">答案</param>
        /// <returns>如果正确则返回用户id, 否则返回-1</returns>
        public IDataReader CheckPasswordAndSecques(string username, string password, bool originalpassword, string secques)
        {

            DbParameter[] parms = {
									   DbHelper.MakeInParam("@username",DbType.AnsiString,20,username),
									   DbHelper.MakeInParam("@password",DbType.AnsiString,32, originalpassword ? Utils.MD5(password) : password),
									   DbHelper.MakeInParam("@secques",DbType.AnsiString,8, secques)
								   };
            string sql = string.Format("SELECT `uid` FROM `{0}users` WHERE `username`=@username AND `password`=@password AND `secques`=@secques LIMIT 0,1", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, sql, parms);
        }

        /// <summary>
        /// 检查密码
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="originalpassword">是否非MD5密码</param>
        /// <returns>如果正确则返回用户id, 否则返回-1</returns>
        public IDataReader CheckPassword(string username, string password, bool originalpassword)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@username",DbType.AnsiString,20, username),
									   DbHelper.MakeInParam("@password",DbType.AnsiString,32, originalpassword ? Utils.MD5(password) : password)
								   };
            string sql = string.Format("SELECT `uid`,`groupid`,`adminid` FROM `dnt_users` WHERE `username`=@username AND `password`=@password LIMIT 0,1", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, sql, parms);
        }

        /// <summary>
        /// 检测DVBBS兼容模式的密码
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="questionid">问题id</param>
        /// <param name="answer">答案</param>
        /// <returns>如果正确则返回用户id, 否则返回-1</returns>
        public IDataReader CheckDvBbsPasswordAndSecques(string username, string password)
        {

            DbParameter[] parms = {
									   DbHelper.MakeInParam("@username",DbType.AnsiString,20,username),
									   DbHelper.MakeInParam("@password",DbType.AnsiString,32, Utils.MD5(password).Substring(8, 16))
								   };
            return DbHelper.ExecuteReader(CommandType.Text, "SELECT TOP 1 [uid], [password], [secques] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [username]=@username", parms);
        }

        /// <summary>
        /// 检测密码
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="password">密码</param>
        /// <param name="originalpassword">是否非MD5密码</param>
        /// <param name="groupid">用户组id</param>
        /// <param name="adminid">管理id</param>
        /// <returns>如果用户密码正确则返回uid, 否则返回-1</returns>
        public IDataReader CheckPassword(int uid, string password, bool originalpassword)
        {

            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid",DbType.Int32,4,uid),
									   DbHelper.MakeInParam("@password",DbType.AnsiString,32, originalpassword ? Utils.MD5(password) : password)
								   };
            string sql = string.Format("SELECT `uid`, `groupid`, `adminid` FROM `{0}users` WHERE `uid`=@uid AND `password`=@password LIMIT 0,1", BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, sql, parms);
        }

        /// <summary>
        /// 根据指定的email查找用户并返回用户uid
        /// </summary>
        /// <param name="email">email地址</param>
        /// <returns>用户uid</returns>
        public IDataReader FindUserEmail(string email)
        {
            System.Diagnostics.Debug.WriteLine("public IDataReader FindUserEmail(string email)");
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@email",DbType.AnsiString,50, email),
								   };
            return DbHelper.ExecuteReader(CommandType.Text, string.Format("SELECT uid FROM {0}users WHERE email=@email LIMIT 0,1", BaseConfigs.GetTablePrefix), parms);
        }

        /// <summary>
        /// 得到论坛中用户总数
        /// </summary>
        /// <returns>用户总数</returns>
        public int GetUserCount()
        {
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(uid) FROM [" + BaseConfigs.GetTablePrefix + "users]"), 0);
        }

        /// <summary>
        /// 得到论坛中用户总数
        /// </summary>
        /// <returns>用户总数</returns>
        public int GetUserCountByAdmin()
        {
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT COUNT(uid) FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [" + BaseConfigs.GetTablePrefix + "users].[adminid] > 0"), 0);
        }

        /// <summary>
        /// 创建新用户.
        /// </summary>
        /// <param name="__userinfo">用户信息</param>
        /// <returns>返回用户ID, 如果已存在该用户名则返回-1</returns>
        public int CreateUser(UserInfo userinfo)
        {
            #region 传参
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@username",DbType.String,20,userinfo.Username),
									   DbHelper.MakeInParam("@nickname",DbType.String,20,userinfo.Nickname),
									   DbHelper.MakeInParam("@password",DbType.AnsiString,32,userinfo.Password),
									   DbHelper.MakeInParam("@secques",DbType.AnsiString,8,userinfo.Secques),
									   DbHelper.MakeInParam("@gender",DbType.Int32,4,userinfo.Gender),
									   DbHelper.MakeInParam("@adminid",DbType.Int32,4,userinfo.Adminid),
									   DbHelper.MakeInParam("@groupid",DbType.Int16,2,userinfo.Groupid),
									   DbHelper.MakeInParam("@groupexpiry",DbType.Int32,4,userinfo.Groupexpiry),
									   DbHelper.MakeInParam("@extgroupids",DbType.AnsiString,60,userinfo.Extgroupids),
									   DbHelper.MakeInParam("@regip",DbType.AnsiString,0,userinfo.Regip),
									   DbHelper.MakeInParam("@joindate",DbType.AnsiString,0,userinfo.Joindate),
									   DbHelper.MakeInParam("@lastip",DbType.AnsiString,15,userinfo.Lastip),
									   DbHelper.MakeInParam("@lastvisit",DbType.AnsiString,0,userinfo.Lastvisit),
									   DbHelper.MakeInParam("@lastactivity",DbType.AnsiString,0,userinfo.Lastactivity),
									   DbHelper.MakeInParam("@lastpost",DbType.AnsiString,0,userinfo.Lastpost),
									   DbHelper.MakeInParam("@lastpostid",DbType.Int32,4,userinfo.Lastpostid),
									   DbHelper.MakeInParam("@lastposttitle",DbType.AnsiString,0,userinfo.Lastposttitle),
									   DbHelper.MakeInParam("@posts",DbType.Int32,4,userinfo.Posts),
									   DbHelper.MakeInParam("@digestposts",DbType.Int16,2,userinfo.Digestposts),
									   DbHelper.MakeInParam("@oltime",DbType.Int32,2,userinfo.Oltime),
									   DbHelper.MakeInParam("@pageviews",DbType.Int32,4,userinfo.Pageviews),
									   DbHelper.MakeInParam("@credits",DbType.Int32,4,userinfo.Credits),
									   DbHelper.MakeInParam("@extcredits1",DbType.Single,8,userinfo.Extcredits1),
									   DbHelper.MakeInParam("@extcredits2",DbType.Single,8,userinfo.Extcredits2),
									   DbHelper.MakeInParam("@extcredits3",DbType.Single,8,userinfo.Extcredits3),
									   DbHelper.MakeInParam("@extcredits4",DbType.Single,8,userinfo.Extcredits4),
									   DbHelper.MakeInParam("@extcredits5",DbType.Single,8,userinfo.Extcredits5),
									   DbHelper.MakeInParam("@extcredits6",DbType.Single,8,userinfo.Extcredits6),
									   DbHelper.MakeInParam("@extcredits7",DbType.Single,8,userinfo.Extcredits7),
									   DbHelper.MakeInParam("@extcredits8",DbType.Single,8,userinfo.Extcredits8),
									   DbHelper.MakeInParam("@avatarshowid",DbType.Int32,4,userinfo.Avatarshowid),
									   DbHelper.MakeInParam("@email",DbType.AnsiString,50,userinfo.Email),
									   DbHelper.MakeInParam("@bday",DbType.AnsiString,0,userinfo.Bday),
									   DbHelper.MakeInParam("@sigstatus",DbType.Int32,4,userinfo.Sigstatus),
									   DbHelper.MakeInParam("@tpp",DbType.Int32,4,userinfo.Tpp),
									   DbHelper.MakeInParam("@ppp",DbType.Int32,4,userinfo.Ppp),
									   DbHelper.MakeInParam("@templateid",DbType.Int16,2,userinfo.Templateid),
									   DbHelper.MakeInParam("@pmsound",DbType.Int32,4,userinfo.Pmsound),
									   DbHelper.MakeInParam("@showemail",DbType.Int32,4,userinfo.Showemail),
									   DbHelper.MakeInParam("@newsletter",DbType.Int32,4,userinfo.Newsletter),
									   DbHelper.MakeInParam("@invisible",DbType.Int32,4,userinfo.Invisible),
									   //DbHelper.MakeInParam("@timeoffset",DbType.AnsiString,4,__userinfo.Timeoffset),
									   DbHelper.MakeInParam("@newpm",DbType.Int32,4,userinfo.Newpm),
									   DbHelper.MakeInParam("@accessmasks",DbType.Int32,4,userinfo.Accessmasks),
									   //
									   DbHelper.MakeInParam("@website",DbType.AnsiString,80,userinfo.Website),
									   DbHelper.MakeInParam("@icq",DbType.AnsiString,12,userinfo.Icq),
									   DbHelper.MakeInParam("@qq",DbType.AnsiString,12,userinfo.Qq),
									   DbHelper.MakeInParam("@yahoo",DbType.AnsiString,40,userinfo.Yahoo),
									   DbHelper.MakeInParam("@msn",DbType.AnsiString,40,userinfo.Msn),
									   DbHelper.MakeInParam("@skype",DbType.AnsiString,40,userinfo.Skype),
									   DbHelper.MakeInParam("@location",DbType.AnsiString,30,userinfo.Location),
									   DbHelper.MakeInParam("@customstatus",DbType.AnsiString,30,userinfo.Customstatus),
									   DbHelper.MakeInParam("@avatar",DbType.AnsiString,255,userinfo.Avatar),
									   DbHelper.MakeInParam("@avatarwidth",DbType.Int32,4,userinfo.Avatarwidth),
									   DbHelper.MakeInParam("@avatarheight",DbType.Int32,4,userinfo.Avatarheight),
									   DbHelper.MakeInParam("@medals",DbType.AnsiString,40, userinfo.Medals),
									   DbHelper.MakeInParam("@bio",DbType.String,500,userinfo.Bio),
									   DbHelper.MakeInParam("@signature",DbType.String,500,userinfo.Signature),
									   DbHelper.MakeInParam("@sightml",DbType.String,1000,userinfo.Sightml),
									   DbHelper.MakeInParam("@authstr",DbType.AnsiString,20,userinfo.Authstr),
                                       DbHelper.MakeInParam("@realname",DbType.String,10,userinfo.Realname),
                                       DbHelper.MakeInParam("@idcard",DbType.AnsiString,20,userinfo.Idcard),
                                       DbHelper.MakeInParam("@mobile",DbType.AnsiString,20,userinfo.Mobile),
                                       DbHelper.MakeInParam("@phone",DbType.AnsiString,20,userinfo.Phone)
								   };
            #endregion
            string sql = string.Format("INSERT INTO `{0}users`(`username`,`nickname`,`password`,`secques`,`gender`,`adminid`,`groupid`,`groupexpiry`,`extgroupids`,`regip`,`joindate`,`lastip`,`lastvisit`,`lastactivity`,`lastpost`,`lastpostid`,`lastposttitle`,`posts`,`digestposts`,`oltime`,`pageviews`,`credits`,`extcredits1`,`extcredits2`,`extcredits3`,`extcredits4`,`extcredits5`,`extcredits6`,`extcredits7`,`extcredits8`,`avatarshowid`,`email`,`bday`,`sigstatus`,`tpp`,`ppp`,`templateid`,`pmsound`,`showemail`,`newsletter`,`invisible`,`newpm`,`accessmasks`) VALUES(@username,@nickname,@password,@secques,@gender,@adminid,@groupid,@groupexpiry,@extgroupids,@regip,@joindate,@lastip,@lastvisit,@lastactivity,@lastpost,@lastpostid,@lastposttitle,@posts,@digestposts,@oltime,@pageviews,@credits,@extcredits1,@extcredits2,@extcredits3,@extcredits4,@extcredits5,@extcredits6,@extcredits7,@extcredits8,@avatarshowid,@email,@bday,@sigstatus,@tpp,@ppp,@templateid,@pmsound,@showemail,@newsletter,@invisible,@newpm,@accessmasks);select last_insert_rowid()", BaseConfigs.GetTablePrefix);
            int uid = Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, parms), -1);
            if (uid > 0)
            {
                DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE {0}statistics SET `totalusers`=`totalusers`+1,`lastusername`=@username,`lastuserid`={1}", BaseConfigs.GetTablePrefix, uid), parms);

                string sql2 = string.Format("INSERT INTO {0}userfields (`uid`,`website`,`icq`,`qq`,`yahoo`,`msn`,`skype`,`location`,`customstatus`,`avatar`,`avatarwidth`,`avatarheight`,`medals`,`bio`,`signature`,`sightml`,`authstr`,`realname`,`idcard`,`mobile`,`phone`) VALUES ({1},@website,@icq,@qq,@yahoo,@msn,@skype,@location,@customstatus,@avatar,@avatarwidth,@avatarheight,@medals,@bio,@signature,@sightml,@authstr,@realname,@idcard,@mobile,@phone)", BaseConfigs.GetTablePrefix, uid);
                DbHelper.ExecuteNonQuery(CommandType.Text, sql2, parms);
            }
            return uid;
        }

        /// <summary>
        /// 更新用户信息.
        /// </summary>
        /// <param name="userinfo">用户信息</param>
        /// <returns>返回是否成功</returns>
        public bool UpdateUser(UserInfo userinfo)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@username",DbType.AnsiString,20,userinfo.Username),
									   DbHelper.MakeInParam("@nickname",DbType.AnsiString,20,userinfo.Nickname),
									   DbHelper.MakeInParam("@password",DbType.AnsiString,32,userinfo.Password),
									   DbHelper.MakeInParam("@secques",DbType.AnsiString,8,userinfo.Secques),
									   DbHelper.MakeInParam("@spaceid",DbType.Int32,4,userinfo.Spaceid),
									   DbHelper.MakeInParam("@gender",DbType.Int32,4,userinfo.Gender),
									   DbHelper.MakeInParam("@adminid",DbType.Int32,4,userinfo.Adminid),
									   DbHelper.MakeInParam("@groupid",DbType.Int16,2,userinfo.Groupid),
									   DbHelper.MakeInParam("@groupexpiry",DbType.String,50,userinfo.Groupexpiry),
									   DbHelper.MakeInParam("@extgroupids",DbType.AnsiString,60,userinfo.Extgroupids),
									   DbHelper.MakeInParam("@regip",DbType.AnsiString,15,userinfo.Regip),
									   DbHelper.MakeInParam("@joindate",DbType.AnsiString,19,userinfo.Joindate),
									   DbHelper.MakeInParam("@lastip",DbType.AnsiString,15,userinfo.Lastip),
									   DbHelper.MakeInParam("@lastvisit",DbType.AnsiString,19,userinfo.Lastvisit),
									   DbHelper.MakeInParam("@lastactivity",DbType.AnsiString,19,userinfo.Lastactivity),
									   DbHelper.MakeInParam("@lastpost",DbType.AnsiString,19,userinfo.Lastpost),
									   DbHelper.MakeInParam("@lastpostid",DbType.Int32,4,userinfo.Lastpostid),
									   DbHelper.MakeInParam("@lastposttitle",DbType.AnsiString,60,userinfo.Lastposttitle),
									   DbHelper.MakeInParam("@posts",DbType.Int32,4,userinfo.Posts),
									   DbHelper.MakeInParam("@digestposts",DbType.Int16,2,userinfo.Digestposts),
									   DbHelper.MakeInParam("@oltime",DbType.Int32,2,userinfo.Oltime),
									   DbHelper.MakeInParam("@pageviews",DbType.Int32,4,userinfo.Pageviews),
									   DbHelper.MakeInParam("@credits",DbType.Int32,4,userinfo.Credits),
									   DbHelper.MakeInParam("@extcredits1",DbType.Single,8,userinfo.Extcredits1),
									   DbHelper.MakeInParam("@extcredits2",DbType.Single,8,userinfo.Extcredits2),
									   DbHelper.MakeInParam("@extcredits3",DbType.Single,8,userinfo.Extcredits3),
									   DbHelper.MakeInParam("@extcredits4",DbType.Single,8,userinfo.Extcredits4),
									   DbHelper.MakeInParam("@extcredits5",DbType.Single,8,userinfo.Extcredits5),
									   DbHelper.MakeInParam("@extcredits6",DbType.Single,8,userinfo.Extcredits6),
									   DbHelper.MakeInParam("@extcredits7",DbType.Single,8,userinfo.Extcredits7),
									   DbHelper.MakeInParam("@extcredits8",DbType.Single,8,userinfo.Extcredits8),
									   DbHelper.MakeInParam("@avatarshowid",DbType.Int32,4,userinfo.Avatarshowid),
									   DbHelper.MakeInParam("@email",DbType.AnsiString,50,userinfo.Email),
									   DbHelper.MakeInParam("@bday",DbType.AnsiString,19,userinfo.Bday),
									   DbHelper.MakeInParam("@sigstatus",DbType.Int32,4,userinfo.Sigstatus),
									   DbHelper.MakeInParam("@tpp",DbType.Int32,4,userinfo.Tpp),
									   DbHelper.MakeInParam("@ppp",DbType.Int32,4,userinfo.Ppp),
									   DbHelper.MakeInParam("@templateid",DbType.Int16,2,userinfo.Templateid),
									   DbHelper.MakeInParam("@pmsound",DbType.Int32,4,userinfo.Pmsound),
									   DbHelper.MakeInParam("@showemail",DbType.Int32,4,userinfo.Showemail),
									   DbHelper.MakeInParam("@newsletter",DbType.Int32,4,userinfo.Newsletter),
									   DbHelper.MakeInParam("@invisible",DbType.Int32,4,userinfo.Invisible),
									   DbHelper.MakeInParam("@newpm",DbType.Int32,4,userinfo.Newpm),
									   DbHelper.MakeInParam("@newpmcount",DbType.Int32,4,userinfo.Newpmcount),
									   DbHelper.MakeInParam("@accessmasks",DbType.Int32,4,userinfo.Accessmasks),
									   DbHelper.MakeInParam("@onlinestate",DbType.Int32,4,userinfo.Onlinestate),
									   DbHelper.MakeInParam("@website",DbType.AnsiString,80,userinfo.Website),
									   DbHelper.MakeInParam("@icq",DbType.AnsiString,12,userinfo.Icq),
									   DbHelper.MakeInParam("@qq",DbType.AnsiString,12,userinfo.Qq),
									   DbHelper.MakeInParam("@yahoo",DbType.AnsiString,40,userinfo.Yahoo),
									   DbHelper.MakeInParam("@msn",DbType.AnsiString,40,userinfo.Msn),
									   DbHelper.MakeInParam("@skype",DbType.AnsiString,40,userinfo.Skype),
									   DbHelper.MakeInParam("@location",DbType.AnsiString,30,userinfo.Location),
									   DbHelper.MakeInParam("@customstatus",DbType.AnsiString,30,userinfo.Customstatus),
									   DbHelper.MakeInParam("@avatar",DbType.AnsiString,255,userinfo.Avatar),
									   DbHelper.MakeInParam("@avatarwidth",DbType.Int32,4,userinfo.Avatarwidth),
									   DbHelper.MakeInParam("@avatarheight",DbType.Int32,4,userinfo.Avatarheight),
									   DbHelper.MakeInParam("@medals",DbType.AnsiString,300, userinfo.Medals),
									   DbHelper.MakeInParam("@bio",DbType.String,500,userinfo.Bio),
									   DbHelper.MakeInParam("@signature",DbType.String,500,userinfo.Signature),
									   DbHelper.MakeInParam("@sightml",DbType.String,1000,userinfo.Sightml),
									   DbHelper.MakeInParam("@authstr",DbType.AnsiString,20,userinfo.Authstr),
									   DbHelper.MakeInParam("@authtime",DbType.DateTime,4,userinfo.Authtime),
									   DbHelper.MakeInParam("@authflag",DbType.Byte,1,userinfo.Authflag),
                                       DbHelper.MakeInParam("@realname",DbType.String,10,userinfo.Realname),
                                       DbHelper.MakeInParam("@idcard",DbType.AnsiString,20,userinfo.Idcard),
                                       DbHelper.MakeInParam("@mobile",DbType.AnsiString,20,userinfo.Mobile),
                                       DbHelper.MakeInParam("@phone",DbType.AnsiString,20,userinfo.Phone),
                                       DbHelper.MakeInParam("@ignorepm",DbType.String,1000,userinfo.Ignorepm),
                                       DbHelper.MakeInParam("@uid",DbType.Int32,4,userinfo.Uid)
								   };

            return Utils.StrToInt(DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "updateuser", parms), -1) == 2;
        }

        /// <summary>
        /// 更新权限验证字符串
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="authstr">验证串</param>
        /// <param name="authflag">验证标志</param>
        public void UpdateAuthStr(int uid, string authstr, int authflag)
        {

            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid", DbType.Int32, 4, uid), 
									   DbHelper.MakeInParam("@authstr", DbType.AnsiString, 20, authstr),
									   DbHelper.MakeInParam("@authflag", DbType.Int32, 4, authflag) 
								   };
            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "updateuserauthstr", parms);
        }

        /// <summary>
        /// 更新指定用户的个人资料
        /// </summary>
        /// <param name="__userinfo">用户信息</param>
        /// <returns>如果用户不存在则为false, 否则为true</returns>
        public void UpdateUserProfile(UserInfo userinfo)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid", DbType.Int32, 4, userinfo.Uid), 
									   DbHelper.MakeInParam("@nickname",DbType.AnsiString,20,userinfo.Nickname),
									   DbHelper.MakeInParam("@gender", DbType.Int32, 4, userinfo.Gender), 
									   DbHelper.MakeInParam("@email", DbType.AnsiString, 50, userinfo.Email), 
									   DbHelper.MakeInParam("@bday", DbType.AnsiString, 10, userinfo.Bday), 
									   DbHelper.MakeInParam("@showemail", DbType.Int32, 4, userinfo.Showemail),
									   DbHelper.MakeInParam("@website", DbType.AnsiString, 80, userinfo.Website), 
									   DbHelper.MakeInParam("@icq", DbType.AnsiString, 12, userinfo.Icq), 
									   DbHelper.MakeInParam("@qq", DbType.AnsiString, 12, userinfo.Qq), 
									   DbHelper.MakeInParam("@yahoo", DbType.AnsiString, 40, userinfo.Yahoo), 
									   DbHelper.MakeInParam("@msn", DbType.AnsiString, 40, userinfo.Msn), 
									   DbHelper.MakeInParam("@skype", DbType.AnsiString, 40, userinfo.Skype), 
									   DbHelper.MakeInParam("@location", DbType.String, 30, userinfo.Location), 
									   DbHelper.MakeInParam("@bio", DbType.String, 500, userinfo.Bio),
                                       DbHelper.MakeInParam("@realname",DbType.String,10,userinfo.Realname),
                                       DbHelper.MakeInParam("@idcard",DbType.AnsiString,20,userinfo.Idcard),
                                       DbHelper.MakeInParam("@mobile",DbType.AnsiString,20,userinfo.Mobile),
                                       DbHelper.MakeInParam("@phone",DbType.AnsiString,20,userinfo.Phone)
								   };

            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "updateuserprofile", parms);
        }

        /// <summary>
        /// 更新用户论坛设置
        /// </summary>
        /// <param name="__userinfo">用户信息</param>
        /// <returns>如果用户不存在则返回false, 否则返回true</returns>
        public void UpdateUserForumSetting(UserInfo userinfo)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid",DbType.Int32,4,userinfo.Uid),
									   DbHelper.MakeInParam("@tpp",DbType.Int32,4,userinfo.Tpp),
									   DbHelper.MakeInParam("@ppp",DbType.Int32,4,userinfo.Ppp),
									   DbHelper.MakeInParam("@invisible",DbType.Int32,4,userinfo.Invisible),
									   DbHelper.MakeInParam("@customstatus",DbType.AnsiString,30,userinfo.Customstatus),
                                       DbHelper.MakeInParam("@sigstatus", DbType.Int32, 4, userinfo.Sigstatus),
                                       DbHelper.MakeInParam("@signature", DbType.String, 500, userinfo.Signature),
                                       DbHelper.MakeInParam("@sightml", DbType.String, 1000, userinfo.Sightml)
								   };

            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "updateuserforumsetting", parms);
        }

        /// <summary>
        /// 修改用户自定义积分字段的值
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="extid">扩展字段序号(1-8)</param>
        /// <param name="pos">增加的数值(可以是负数)</param>
        /// <returns>执行是否成功</returns>
        public void UpdateUserExtCredits(int uid, int extid, float pos)
        {
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [extcredits" + extid.ToString() + "]=[extcredits" + extid.ToString() + "] + (" + pos.ToString() + ") WHERE [uid]=" + uid.ToString());
        }

        /// <summary>
        /// 获得指定用户的指定积分扩展字段的值
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="extid">扩展字段序号(1-8)</param>
        /// <returns>值</returns>
        public float GetUserExtCredits(int uid, int extid)
        {
            return Utils.StrToFloat(DbHelper.ExecuteDataset(CommandType.Text, "SELECT [extcredits" + extid.ToString() + "] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid]=" + uid.ToString()).Tables[0].Rows[0][0], 0);
        }

        ///// <summary>
        ///// 更新用户签名
        ///// </summary>
        ///// <param name="uid">用户id</param>
        ///// <param name="signature">签名</param>
        ///// <returns>如果用户不存在则返回false, 否则返回true</returns>
        //public void UpdateUserSignature(int uid, int sigstatus, string signature, string sightml)
        //{
        //    DbParameter[] parms = {
        //                               DbHelper.MakeInParam("@uid",DbType.Int32,4,uid),
        //                               DbHelper.MakeInParam("@sigstatus",DbType.Int32,4,sigstatus),
        //                               DbHelper.MakeInParam("@signature",DbType.String,500,signature),
        //                               DbHelper.MakeInParam("@sightml",DbType.String,1000,sightml)
        //                           };
        //    DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "updateusersignature", parms);
        //}

        /// <summary>
        /// 更新用户头像
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="avatar">头像</param>
        /// <param name="avatarwidth">头像宽度</param>
        /// <param name="avatarheight">头像高度</param>
        /// <returns>如果用户不存在则返回false, 否则返回true</returns>
        public void UpdateUserPreference(int uid, string avatar, int avatarwidth, int avatarheight, int templateid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid",DbType.Int32,4,uid),
									   DbHelper.MakeInParam("@avatar",DbType.AnsiString,255,avatar),
									   DbHelper.MakeInParam("@avatarwidth",DbType.Int32,4,avatarwidth),
									   DbHelper.MakeInParam("@avatarheight",DbType.Int32,4,avatarheight),
                                       DbHelper.MakeInParam("@templateid", DbType.Int16, 4, templateid)
								   };

            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "updateuserpreference", parms);
        }

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="password">密码</param>
        /// <param name="originalpassword">是否非MD5密码</param>
        /// <returns>成功返回true否则false</returns>
        public void UpdateUserPassword(int uid, string password, bool originalpassword)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid", DbType.Int32, 4, uid), 
									   DbHelper.MakeInParam("@password", DbType.AnsiString, 32, originalpassword ? Utils.MD5(password) : password)
								   };

            DbHelper.ExecuteNonQuery(CommandType.StoredProcedure, BaseConfigs.GetTablePrefix + "updateuserpassword", parms);
        }

        /// <summary>
        /// 更新用户安全问题
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="questionid">问题id</param>
        /// <param name="answer">答案</param>
        /// <returns>成功返回true否则false</returns>
        public void UpdateUserSecques(int uid, string secques)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid", DbType.Int32, 4, uid), 
									   DbHelper.MakeInParam("@secques", DbType.AnsiString, 8, secques)
								   };

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [secques]=@secques WHERE [uid]=@uid", parms);
        }

        /// <summary>
        /// 更新用户最后登录时间
        /// </summary>
        /// <param name="uid">用户id</param>
        public void UpdateUserLastvisit(int uid, string ip)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid", DbType.Int32, 4, uid),
									   DbHelper.MakeInParam("@ip", DbType.AnsiString,15, ip)
								   };
            string sql = string.Format("UPDATE `{0}users` SET `lastvisit`=datetime('now','localtime'), `lastip`=@ip WHERE `uid`=@uid", BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public void UpdateUserOnlineStateAndLastActivity(string uidlist, int onlinestate, string activitytime)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@onlinestate", DbType.Int32, 4, onlinestate),
									    DbHelper.MakeInParam("@activitytime", DbType.DateTime, 8, DateTime.Parse(activitytime))
								   };
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [onlinestate]=@onlinestate,[lastactivity] = @activitytime WHERE [uid] IN (" + uidlist + ")", parms);
        }

        public void UpdateUserOnlineStateAndLastActivity(int uid, int onlinestate, string activitytime)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@uid", DbType.Int32, 4, uid),
                                        DbHelper.MakeInParam("@onlinestate", DbType.Int32, 4, onlinestate),
									    DbHelper.MakeInParam("@activitytime", DbType.DateTime, 8, DateTime.Parse(activitytime))
								   };
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [onlinestate]=@onlinestate,[lastactivity] = @activitytime WHERE [uid]=@uid", parms);
        }

        public void UpdateUserOnlineStateAndLastVisit(string uidlist, int onlinestate, string activitytime)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@onlinestate", DbType.Int32, 4, onlinestate),
									    DbHelper.MakeInParam("@activitytime", DbType.DateTime, 8, DateTime.Parse(activitytime))
								   };
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [onlinestate]=@onlinestate,[lastvisit] = @activitytime WHERE [uid] IN (" + uidlist + ")", parms);
        }

        public void UpdateUserOnlineStateAndLastVisit(int uid, int onlinestate, string activitytime)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@uid", DbType.Int32, 4, uid),
                                        DbHelper.MakeInParam("@onlinestate", DbType.Int32, 4, onlinestate),
									    DbHelper.MakeInParam("@activitytime", DbType.DateTime, 8, DateTime.Parse(activitytime))
								   };
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [onlinestate]=@onlinestate,[lastvisit] = @activitytime WHERE [uid]=@uid", parms);
        }

        /// <summary>
        /// 更新用户当前的在线时间和最后活动时间
        /// </summary>
        /// <param name="uid">用户uid</param>
        public void UpdateUserLastActivity(int uid, string activitytime)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid", DbType.Int32, 4, uid),
									   DbHelper.MakeInParam("@activitytime", DbType.DateTime, 8, DateTime.Parse(activitytime))
								   };


            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [lastactivity] = @activitytime  WHERE [uid] = @uid", parms);

        }

        /// <summary>
        /// 设置用户信息表中未读短消息的数量
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="pmnum">短消息数量</param>
        /// <returns>更新记录个数</returns>
        public int SetUserNewPMCount(int uid, int pmnum)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid", DbType.Int32, 4, uid), 
									   DbHelper.MakeInParam("@value", DbType.Int32, 4, pmnum)
			};
            return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [newpmcount]=@value WHERE [uid]=@uid", parms);
        }

        /// <summary>
        /// 更新指定用户的勋章信息
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <param name="medals">勋章信息</param>
        public void UpdateMedals(int uid, string medals)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid", DbType.Int32, 4, uid), 
									   DbHelper.MakeInParam("@medals", DbType.AnsiString, 300, medals)
								   };
            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "userfields] SET [medals]=@medals WHERE [uid]=@uid", parms);

        }

        public int DecreaseNewPMCount(int uid, int subval)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid", DbType.Int32, 4, uid), 
									   DbHelper.MakeInParam("@subval", DbType.Int32, 4, subval)
			};

            try
            {
                return DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [newpmcount]=CASE WHEN [newpmcount] >= 0 THEN [newpmcount]-@subval ELSE 0 END WHERE [uid]=@uid", parms);
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 得到用户新短消息数量
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>新短消息数</returns>
        public int GetUserNewPMCount(int uid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@uid", DbType.Int32, 4, uid), 
			};
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, "SELECT [newpmcount] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid]=@uid", parms), 0);
        }

        /// <summary>
        /// 更新用户精华数
        /// </summary>
        /// <param name="useridlist">uid列表</param>
        /// <returns></returns>
        public int UpdateUserDigest(string useridlist)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [digestposts] = (");
            sql.Append("SELECT COUNT([tid]) AS [digest] FROM [" + BaseConfigs.GetTablePrefix + "topics] WHERE [" + BaseConfigs.GetTablePrefix + "topics].[posterid] = [" + BaseConfigs.GetTablePrefix + "users].[uid] AND [digest]>0");
            sql.Append(") WHERE [uid] IN (");
            sql.Append(useridlist);
            sql.Append(")");

            return DbHelper.ExecuteNonQuery(CommandType.Text, sql.ToString());
        }

        /// <summary>
        /// 更新用户SpaceID
        /// </summary>
        /// <param name="spaceid">要更新的SpaceId</param>
        /// <param name="userid">要更新的UserId</param>
        /// <returns></returns>
        public void UpdateUserSpaceId(int spaceid, int userid)
        {
            DbParameter[] parms = {
									   DbHelper.MakeInParam("@spaceid",DbType.Int32,4,spaceid),
									   DbHelper.MakeInParam("@uid",DbType.Int32,4,userid)
								   };
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [spaceid]=@spaceid WHERE [uid]=@uid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public DataTable GetUserIdByAuthStr(string authstr)
        {
            DbParameter[] parms = {
										  DbHelper.MakeInParam("@authstr",DbType.AnsiString,20,authstr)
				};

            DataTable dt = DbHelper.ExecuteDataset(CommandType.Text, "SELECT [uid] FROM [" + BaseConfigs.GetTablePrefix + "userfields] WHERE DateDiff(d,[authtime],getdate())<=3  AND [authstr]=@authstr", parms).Tables[0];

            return dt;
        }

        /// <summary>
        /// 执行在线用户向表及缓存中添加的操作。
        /// </summary>
        /// <param name="onlineuserinfo">在组用户信息内容</param>
        /// <returns>添加成功则返回刚刚添加的olid,失败则返回0</returns>
        public int AddOnlineUser(OnlineUserInfo onlineuserinfo, int timeout)
        {
            System.Diagnostics.Debug.WriteLine("AddOnlineUser()");
            //标识需要更新用户在线状态，0表示需要更新
            int onlinestate = 1;
            //string strDelTimeOutSql = "";
            // 如果timeout为负数则代表不需要精确更新用户是否在线的状态
            if (timeout > 0)
            {
                if (onlineuserinfo.Userid > 0)
                {
                    onlinestate = 0;
                    //strDelTimeOutSql = string.Format("{0}UPDATE [{1}users] SET [onlinestate]=1 WHERE [uid]={2};", strDelTimeOutSql, BaseConfigs.GetTablePrefix, onlineuserinfo.Userid.ToString());
                }
            }
            else
            {
                timeout = timeout * -1;
            }

            if (timeout > 9999)
            {
                timeout = 9999;
            }





            DbParameter[] parms = {
									   DbHelper.MakeInParam("@onlinestate",DbType.Int32,4,onlinestate),
									   DbHelper.MakeInParam("@userid",DbType.Int32,4,onlineuserinfo.Userid),
									   DbHelper.MakeInParam("@ip",DbType.AnsiString,15,onlineuserinfo.Ip),
									   DbHelper.MakeInParam("@username",DbType.String,40,onlineuserinfo.Username),
									   DbHelper.MakeInParam("@nickname",DbType.String,40,onlineuserinfo.Nickname),
									   DbHelper.MakeInParam("@password",DbType.AnsiString,32,onlineuserinfo.Password),
									   DbHelper.MakeInParam("@groupid",DbType.Int16,2,onlineuserinfo.Groupid),
									   DbHelper.MakeInParam("@olimg",DbType.AnsiString,80,onlineuserinfo.Olimg),
									   DbHelper.MakeInParam("@adminid",DbType.Int16,2,onlineuserinfo.Adminid),
									   DbHelper.MakeInParam("@invisible",DbType.Int16,2,onlineuserinfo.Invisible),
									   DbHelper.MakeInParam("@action",DbType.Int16,2,onlineuserinfo.Action),
									   DbHelper.MakeInParam("@lastactivity",DbType.Int16,2,onlineuserinfo.Lastactivity),
									   DbHelper.MakeInParam("@lastposttime",DbType.DateTime,8,DateTime.Parse(onlineuserinfo.Lastposttime)),
									   DbHelper.MakeInParam("@lastpostpmtime",DbType.DateTime,8,DateTime.Parse(onlineuserinfo.Lastpostpmtime)),
									   DbHelper.MakeInParam("@lastsearchtime",DbType.DateTime,8,DateTime.Parse(onlineuserinfo.Lastsearchtime)),
									   DbHelper.MakeInParam("@lastupdatetime",DbType.DateTime,8,DateTime.Parse(onlineuserinfo.Lastupdatetime)),
									   DbHelper.MakeInParam("@forumid",DbType.Int32,4,onlineuserinfo.Forumid),
									   DbHelper.MakeInParam("@forumname",DbType.String,50,""),
									   DbHelper.MakeInParam("@titleid",DbType.Int32,4,onlineuserinfo.Titleid),
									   DbHelper.MakeInParam("@title",DbType.String,80,""),
									   DbHelper.MakeInParam("@verifycode",DbType.AnsiString,10,onlineuserinfo.Verifycode),
									   DbHelper.MakeInParam("@newpms",DbType.Int16,2,onlineuserinfo.Newpms),
									   DbHelper.MakeInParam("@newnotices",DbType.Int16,2,onlineuserinfo.Newnotices)
								   };
            //int olid = Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, strDelTimeOutSql + "INSERT INTO [" + BaseConfigs.GetTablePrefix + "online] ([userid],[ip],[username],[nickname],[password],[groupid],[olimg],[adminid],[invisible],[action],[lastactivity],[lastposttime],[lastpostpmtime],[lastsearchtime],[lastupdatetime],[forumid],[forumname],[titleid],[title],[verifycode],[newpms],[newnotices])VALUES(@userid,@ip,@username,@nickname,@password,@groupid,@olimg,@adminid,@invisible,@action,@lastactivity,@lastposttime,@lastpostpmtime,@lastsearchtime,@lastupdatetime,@forumid,@forumname,@titleid,@title,@verifycode,@newpms,@newnotices);SELECT SCOPE_IDENTITY()", parms).ToString(), 0);
            if (onlinestate == 0)
            {
                DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}users] SET [onlinestate]=1 WHERE [uid]=@userid", BaseConfigs.GetTablePrefix), parms);
            }
            string sql = string.Format("INSERT INTO [{0}online] ([userid],[ip],[username],[nickname],[password],[groupid],[olimg],[adminid],[invisible],[action],[lastactivity],[lastposttime],[lastpostpmtime],[lastsearchtime],[lastupdatetime],[forumid],[forumname],[titleid],[title],[verifycode],[newpms],[newnotices])VALUES(@userid,@ip,@username,@nickname,@password,@groupid,@olimg,@adminid,@invisible,@action,@lastactivity,@lastposttime,@lastpostpmtime,@lastsearchtime,@lastupdatetime,@forumid,@forumname,@titleid,@title,@verifycode,@newpms,@newnotices);SELECT last_insert_rowid()", BaseConfigs.GetTablePrefix);
            int olid = Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, parms), 0);

            //系统中间隔5分钟之内不清除过期用户
            if (_lastRemoveTimeout == 0 || (System.Environment.TickCount - _lastRemoveTimeout) > 300000)
            {
                DeleteExpiredOnlineUsers(timeout);
                _lastRemoveTimeout = System.Environment.TickCount;
            }
            // 如果id值太大则重建在线表
            if (olid > 2147483000)
            {
                CreateOnlineTable();
                //DbHelper.ExecuteNonQuery(CommandType.Text, strDelTimeOutSql + "INSERT INTO [" + BaseConfigs.GetTablePrefix + "online] ([userid],[ip],[username],[nickname],[password],[groupid],[olimg],[adminid],[invisible],[action],[lastactivity],[lastposttime],[lastpostpmtime],[lastsearchtime],[lastupdatetime],[forumid],[titleid],[verifycode])VALUES(@userid,@ip,@username,@nickname,@password,@groupid,@olimg,@adminid,@invisible,@action,@lastactivity,@lastposttime,@lastpostpmtime,@lastsearchtime,@lastupdatetime,@forumid,@forumname,@titleid,@title,@verifycode);SELECT SCOPE_IDENTITY()", parms);
                if (onlinestate == 0)
                {
                    DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}users] SET [onlinestate]=1 WHERE [uid]=@userid", BaseConfigs.GetTablePrefix), parms);
                }
                DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
                return 1;
            }


            return 0;
            //return (int)DbHelper.ExecuteDataset(CommandType.Text, "SELECT [olid] FROM ["+BaseConfigFactory.GetTablePrefix+"online] WHERE [userid]=" + __onlineuserinfo.Userid.ToString()).Tables[0].Rows[0][0];

        }

        private void DeleteExpiredOnlineUsers(int timeout)
        {
            System.Text.StringBuilder timeoutStrBuilder = new System.Text.StringBuilder();
            System.Text.StringBuilder memberStrBuilder = new System.Text.StringBuilder();

            //string strDelTimeOutSql = "";
            DbParameter param = DbHelper.MakeInParam("@expires", DbType.DateTime, 8, DateTime.Now.AddMinutes(timeout * -1));
            string sql = string.Format("SELECT `olid`, `userid` FROM `{0}online` WHERE `lastupdatetime`<@expires", BaseConfigs.GetTablePrefix);
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, sql, param);
            while (dr.Read())
            {
                timeoutStrBuilder.Append(",");
                timeoutStrBuilder.Append(dr["olid"].ToString());
                if (dr["userid"].ToString() != "-1")
                {
                    memberStrBuilder.Append(",");
                    memberStrBuilder.Append(dr["userid"].ToString());
                }
            }
            dr.Close();

            if (timeoutStrBuilder.Length > 0)
            {
                timeoutStrBuilder.Remove(0, 1);
                //strDelTimeOutSql = string.Format("DELETE FROM [{0}online] WHERE [olid] IN ({1});", BaseConfigs.GetTablePrefix, timeoutStrBuilder.ToString());
                string sql2 = string.Format("DELETE FROM `{0}online` WHERE `olid` IN ({1})", BaseConfigs.GetTablePrefix, timeoutStrBuilder.ToString());
                DbHelper.ExecuteNonQuery(CommandType.Text, sql2);
            }
            if (memberStrBuilder.Length > 0)
            {
                memberStrBuilder.Remove(0, 1);
                //strDelTimeOutSql = string.Format("{0}UPDATE [{1}users] SET [onlinestate]=0,[lastactivity]=GETDATE() WHERE [uid] IN ({2});", strDelTimeOutSql, BaseConfigs.GetTablePrefix, memberStrBuilder.ToString());
                string sql3 = string.Format("UPDATE `{0}users` SET `onlinestate`=0,`lastactivity`=datetime('now','localtime') WHERE `uid` IN ({1})", BaseConfigs.GetTablePrefix, memberStrBuilder.ToString());
                DbHelper.ExecuteNonQuery(CommandType.Text, sql3);
            }
            //if (strDelTimeOutSql != string.Empty)
            //    DbHelper.ExecuteNonQuery(strDelTimeOutSql);
        }

        public DataTable GetUserInfo(int userid)
        {
            DbParameter parm = DbHelper.MakeInParam("@uid", DbType.Int32, 4, userid);
            string sql = "SELECT TOP 1 * FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [uid]=@uid";
            return DbHelper.ExecuteDataset(CommandType.Text, sql, parm).Tables[0];
        }

        public DataTable GetUserInfo(string username, string password)
        {
            DbParameter[] parms = {
                                        DbHelper.MakeInParam("@username", DbType.AnsiString, 20, username),
                                        DbHelper.MakeInParam("@password", DbType.AnsiString, 32, password)
                                    };
            string sql = "SELECT TOP 1 * FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [username]=@username AND [password]=@password";

            return DbHelper.ExecuteDataset(CommandType.Text, sql, parms).Tables[0];
        }

        public void UpdateUserSpaceId(int userid)
        {
            DbParameter parm = DbHelper.MakeInParam("@userid", DbType.Int32, 4, userid);
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [spaceid]=ABS([spaceid]) WHERE [uid]=@userid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parm);
        }

        public int GetUserIdByRewriteName(string rewritename)
        {
            DbParameter parm = DbHelper.MakeInParam("@rewritename", DbType.AnsiString, 100, rewritename);
            string sql = string.Format("SELECT [userid] FROM [{0}spaceconfigs] WHERE [rewritename]=@rewritename", BaseConfigs.GetTablePrefix);
            return Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, parm), -1);
        }

        public void UpdateUserPMSetting(UserInfo user)
        {
            DbParameter[] parms = {
                                    DbHelper.MakeInParam("@uid", DbType.Int32, 4, user.Uid),
                                    DbHelper.MakeInParam("@pmsound", DbType.Int32, 4, user.Pmsound),
                                    DbHelper.MakeInParam("@newsletter", DbType.Int32, 4, (int)user.Newsletter)
                                };

            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format(@"UPDATE [{0}users] SET [pmsound]=@pmsound, [newsletter]=@newsletter WHERE [uid]=@uid", BaseConfigs.GetTablePrefix), parms);

            parms = new DbParameter[] {
                                    DbHelper.MakeInParam("@ignorepm", DbType.String, 1000, user.Ignorepm),
                                    DbHelper.MakeInParam("@uid", DbType.Int32, 4, user.Uid)
                                  };

            DbHelper.ExecuteNonQuery(CommandType.Text, string.Format(@"UPDATE [{0}userfields] SET [ignorepm]=@ignorepm WHERE [uid]=@uid", BaseConfigs.GetTablePrefix), parms);
        }

        public void ClearUserSpace(int uid)
        {
            DbParameter parm = DbHelper.MakeInParam("@uid", DbType.Int32, 4, uid);
            string sql = string.Format("UPDATE [{0}users] SET [spaceid]=0 WHERE [uid]=@uid", BaseConfigs.GetTablePrefix);
            DbHelper.ExecuteNonQuery(CommandType.Text, sql, parm);
        }


        public IDataReader GetUserInfoByName(string username)
        {
            //DbParameter parm =DbHelper.MakeInParam("@username", DbType.AnsiString, 20, username);
            return DbHelper.ExecuteReader(CommandType.Text, "SELECT [uid], [username] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE [username] LIKE '%" + RegEsc(username) + "%'");
        }


        public DataTable UserList(int pagesize, int currentpage, string condition)
        {
            #region 获得用户列表

            int pagetop = (currentpage - 1) * pagesize;

            if (currentpage == 1)
            {
                return DbHelper.ExecuteDataset("SELECT TOP " + pagesize.ToString() + " [" + BaseConfigs.GetTablePrefix + "users].[uid], [" + BaseConfigs.GetTablePrefix + "users].[username],[" + BaseConfigs.GetTablePrefix + "users].[nickname], [" + BaseConfigs.GetTablePrefix + "users].[joindate], [" + BaseConfigs.GetTablePrefix + "users].[credits], [" + BaseConfigs.GetTablePrefix + "users].[posts], [" + BaseConfigs.GetTablePrefix + "users].[lastactivity], [" + BaseConfigs.GetTablePrefix + "users].[email],[" + BaseConfigs.GetTablePrefix + "users].[lastvisit],[" + BaseConfigs.GetTablePrefix + "users].[lastvisit],[" + BaseConfigs.GetTablePrefix + "users].[accessmasks], [" + BaseConfigs.GetTablePrefix + "userfields].[location],[" + BaseConfigs.GetTablePrefix + "usergroups].[grouptitle] FROM [" + BaseConfigs.GetTablePrefix + "users] LEFT JOIN [" + BaseConfigs.GetTablePrefix + "userfields] ON [" + BaseConfigs.GetTablePrefix + "userfields].[uid] = [" + BaseConfigs.GetTablePrefix + "users].[uid]  LEFT JOIN [" + BaseConfigs.GetTablePrefix + "usergroups] ON [" + BaseConfigs.GetTablePrefix + "usergroups].[groupid]=[" + BaseConfigs.GetTablePrefix + "users].[groupid] WHERE " + condition + " ORDER BY [" + BaseConfigs.GetTablePrefix + "users].[uid] DESC").Tables[0];
            }
            else
            {
                string sqlstring = "SELECT TOP " + pagesize.ToString() + " [" + BaseConfigs.GetTablePrefix + "users].[uid], [" + BaseConfigs.GetTablePrefix + "users].[username],[" + BaseConfigs.GetTablePrefix + "users].[nickname], [" + BaseConfigs.GetTablePrefix + "users].[joindate], [" + BaseConfigs.GetTablePrefix + "users].[credits], [" + BaseConfigs.GetTablePrefix + "users].[posts], [" + BaseConfigs.GetTablePrefix + "users].[lastactivity], [" + BaseConfigs.GetTablePrefix + "users].[email],[" + BaseConfigs.GetTablePrefix + "users].[lastvisit],[" + BaseConfigs.GetTablePrefix + "users].[lastvisit],[" + BaseConfigs.GetTablePrefix + "users].[accessmasks], [" + BaseConfigs.GetTablePrefix + "userfields].[location],[" + BaseConfigs.GetTablePrefix + "usergroups].[grouptitle] FROM [" + BaseConfigs.GetTablePrefix + "users],[" + BaseConfigs.GetTablePrefix + "userfields],[" + BaseConfigs.GetTablePrefix + "usergroups]  WHERE [" + BaseConfigs.GetTablePrefix + "userfields].[uid] = [" + BaseConfigs.GetTablePrefix + "users].[uid] AND  [" + BaseConfigs.GetTablePrefix + "usergroups].[groupid]=[" + BaseConfigs.GetTablePrefix + "users].[groupid] AND [" + BaseConfigs.GetTablePrefix + "users].[uid] < (SELECT min([uid])  FROM (SELECT TOP " + pagetop + " [uid] FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE " + condition + " ORDER BY [uid] DESC) AS tblTmp ) AND " + condition + " ORDER BY [" + BaseConfigs.GetTablePrefix + "users].[uid] DESC";
                return DbHelper.ExecuteDataset(sqlstring).Tables[0];
            }

            #endregion
        }
        public void LessenTotalUsers()
        {

            DbHelper.ExecuteNonQuery(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "statistics] SET [totalusers]=[totalusers]-1");
        }

        public void UpdateOnlineTime(int oltimespan, int uid)
        {
            DbParameter[] parms = {
                                    DbHelper.MakeInParam("@uid", DbType.Int32, 4, uid),
                                    DbHelper.MakeInParam("@oltimespan", DbType.Int16, 2, oltimespan),
                                    DbHelper.MakeInParam("@lastupdate", DbType.DateTime, 8, DateTime.Now),
                                    DbHelper.MakeInParam("@expectedlastupdate", DbType.DateTime, 8, DateTime.Now.AddMinutes(0 - oltimespan))
                                };
            string sql = string.Format("UPDATE [{0}onlinetime] SET [thismonth]=[thismonth]+@oltimespan, [total]=[total]+@oltimespan, [lastupdate]=@lastupdate WHERE [uid]=@uid AND [lastupdate]<=@expectedlastupdate", BaseConfigs.GetTablePrefix);
            if (DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms) < 1)
            {
                try
                {
                    DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("INSERT INTO [{0}onlinetime]([uid], [thismonth], [total], [lastupdate]) VALUES(@uid, @oltimespan, @oltimespan, @lastupdate)", BaseConfigs.GetTablePrefix), parms);
                }
                catch
                { }
            }
        }

        /// <summary>
        /// 重置每月在线时间(清零)
        /// </summary>
        public void ResetThismonthOnlineTime()
        {
            DbHelper.ExecuteNonQuery(string.Format("UPDATE [{0}onlinetime] SET [thismonth]=0", BaseConfigs.GetTablePrefix));
        }

        public void SynchronizeOltime(int uid)
        {
            DbParameter[] parms = {
                                    DbHelper.MakeInParam("@uid", DbType.Int32, 4, uid),
                                };
            string sql = string.Format("SELECT [total] FROM [{0}onlinetime] WHERE [uid]=@uid", BaseConfigs.GetTablePrefix);
            int total = Utils.StrToInt(DbHelper.ExecuteScalar(CommandType.Text, sql, parms), 0);
            
            if (DbHelper.ExecuteNonQuery(CommandType.Text, string.Format("UPDATE [{0}users] SET [oltime]={1} WHERE [oltime]<{1} AND [uid]=@uid", BaseConfigs.GetTablePrefix, total), parms) < 1)
            {

                try
                {
                    sql = string.Format("UPDATE [{0}onlinetime] SET [total]=(SELECT [oltime] FROM [{0}users] WHERE [uid]=@uid) WHERE [uid]=@uid", BaseConfigs.GetTablePrefix);
                    DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
                }
                catch
                {
                }
            }
        }

        public IDataReader GetUserByOnlineTime(string field)
        {
            string commandText = string.Format("SELECT TOP 20 [o].[uid], [u].[username], [o].[{0}] FROM [{1}onlinetime] [o] LEFT JOIN [{1}users] [u] ON [o].[uid]=[u].[uid] ORDER BY [o].[{0}] DESC", field, BaseConfigs.GetTablePrefix);
            return DbHelper.ExecuteReader(CommandType.Text, commandText);
        }

        public DataTable GetUserField(int uid)
        {
            string sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "userfields] WHERE [uid] = " + uid;
            return DbHelper.ExecuteDataset(sql).Tables[0];
        }

        public void UpdateBanUser(int groupid,string groupexpiry,int uid)
        {
            DbParameter[] parms = {
                                    DbHelper.MakeInParam("@groupid", DbType.Int32, 4, groupid),
                                    DbHelper.MakeInParam("@groupexpiry", DbType.String, 50, groupexpiry),
                                    DbHelper.MakeInParam("@uid", DbType.Int32, 4, uid)
                                };
            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [groupid]=@groupid, [groupexpiry]=@groupexpiry WHERE [uid]=@uid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql,parms);
        }


        public DataTable SearchSpecialUser(int fid)
        {
            DbParameter[] parms = { 
                                     DbHelper.MakeInParam("@fid",DbType.Int32,4,fid)
                                  };

            string sql = "SELECT * FROM [" + BaseConfigs.GetTablePrefix + "forums] f1 JOIN [" + BaseConfigs.GetTablePrefix + "forumfields] f2 ON [f1].[fid]=[f2].[fid] where [f1].[fid]=@fid";
            
            return DbHelper.ExecuteDataset(CommandType.Text, sql,parms).Tables[0];
        }

        public void UpdateSpecialUser(string permuserlist, int fid)
        {
            DbParameter[] parms = { 
                                     DbHelper.MakeInParam("@permuserlist",DbType.String, 0, permuserlist),
                                     DbHelper.MakeInParam("@fid",DbType.Int32, 4, fid)
                                  };

            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "forumfields] SET [permuserlist]=@permuserlist WHERE [fid]=@fid";
            DbHelper.ExecuteNonQuery(CommandType.Text, sql,parms);
        }

        public int UpdateNewPms(int olid, int pluscount)
        {
            DbParameter[] parms = { 
                                     DbHelper.MakeInParam("@pluscount",DbType.Int16, 2, short.Parse(pluscount.ToString())),
                                     DbHelper.MakeInParam("@olid",DbType.Int32, 4, olid)
                                  };

            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "online] SET [newpms]=[newpms]+@pluscount WHERE [olid]=@olid";
            return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        public int UpdateNewNotices(int olid, int pluscount)
        {
            DbParameter[] parms = { 
                                     DbHelper.MakeInParam("@pluscount",DbType.Int16, 2, short.Parse(pluscount.ToString())),
                                     DbHelper.MakeInParam("@olid",DbType.Int32, 4, olid)
                                  };

            string sql = "UPDATE [" + BaseConfigs.GetTablePrefix + "online] SET [newnotices]=[newnotices]+@pluscount WHERE [olid]=@olid";
            return DbHelper.ExecuteNonQuery(CommandType.Text, sql, parms);
        }

        /// <summary>
        /// 得到指定用户的指定积分扩展字段的积分值
        /// </summary>
        /// <returns>扩展字展积分值</returns>
        public int GetUserExtCreditsByUserid(int uid, int extnumber)
        {
            return Convert.ToInt32(Utils.StrToFloat(DbHelper.ExecuteScalar(CommandType.Text, string.Format("SELECT TOP 1 [extcredits{0}] FROM [{1}users] WHERE [uid] = {2}", extnumber, BaseConfigs.GetTablePrefix, uid)), 0));
        }

    }
}