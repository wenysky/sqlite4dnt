DROP TABLE IF EXISTS `dnt_forums`;
CREATE TABLE `dnt_forums` (
	`fid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`parentid` INTEGER   NOT NULL DEFAULT '0',
	`layer` INTEGER   NOT NULL DEFAULT '0',
	`pathlist` CHAR(3000)   NOT NULL DEFAULT '',
	`parentidlist` CHAR(300)   NOT NULL ,
	`subforumcount` INTEGER   NOT NULL DEFAULT 1,
	`name` CHAR(50)   NOT NULL ,
	`status` INTEGER   NOT NULL DEFAULT '0',
	`displayorder` INTEGER   NOT NULL DEFAULT '0',
	`templateid` INTEGER   NOT NULL DEFAULT '0',
	`topics` INTEGER   NOT NULL DEFAULT '0',
	`curtopics` INTEGER   NOT NULL DEFAULT '0',
	`posts` INTEGER   NOT NULL DEFAULT '0',
	`todayposts` INTEGER   NOT NULL DEFAULT '0',
	`lasttid` INTEGER   NOT NULL DEFAULT '0',
	`lasttitle` CHAR(60)   NOT NULL DEFAULT '',
	`lastpost` DATETIME   NOT NULL DEFAULT '',
	`lastposterid` INTEGER   NOT NULL DEFAULT '',
	`lastposter` CHAR(20)   NOT NULL DEFAULT '',
	`allowsmilies` INTEGER   NOT NULL DEFAULT '0',
	`allowrss` INTEGER   NOT NULL DEFAULT '0',
	`allowhtml` INTEGER   NOT NULL DEFAULT '0',
	`allowbbcode` INTEGER   NOT NULL DEFAULT '0',
	`allowimgcode` INTEGER   NOT NULL DEFAULT '0',
	`allowblog` INTEGER   NOT NULL DEFAULT '0',
	`istrade` INTEGER   NOT NULL DEFAULT '0',
	`allowpostspecial` INTEGER   NOT NULL DEFAULT 0,
	`allowspecialonly` INTEGER   NOT NULL DEFAULT 0,
	`alloweditrules` INTEGER   NOT NULL DEFAULT '0',
	`allowthumbnail` INTEGER   NOT NULL DEFAULT '0',
	`allowtag` INTEGER   NOT NULL DEFAULT 0,
	`recyclebin` INTEGER   NOT NULL DEFAULT '0',
	`modnewposts` INTEGER   NOT NULL DEFAULT '0',
	`jammer` INTEGER   NOT NULL DEFAULT '0',
	`disablewatermark` INTEGER   NOT NULL DEFAULT '0',
	`inheritedmod` INTEGER   NOT NULL DEFAULT '0',
	`autoclose` INTEGER   NOT NULL DEFAULT '0',
	`colcount` INTEGER   NOT NULL DEFAULT 1
);

DROP TABLE IF EXISTS `dnt_templates`;
CREATE TABLE `dnt_templates` (
	`templateid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`directory` VARCHAR(100)   NOT NULL ,
	`name` VARCHAR(50)   NOT NULL ,
	`author` VARCHAR(100)   NOT NULL ,
	`createdate` VARCHAR(50)   NOT NULL ,
	`ver` VARCHAR(100)   NOT NULL ,
	`fordntver` VARCHAR(100)   NOT NULL ,
	`copyright` VARCHAR(100)   NOT NULL 
);

DROP TABLE IF EXISTS `dnt_attachpaymentlog`;
CREATE TABLE `dnt_attachpaymentlog` (
	`id` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`uid` INTEGER   NOT NULL DEFAULT 0,
	`username` CHAR(20)   NOT NULL DEFAULT '',
	`aid` INTEGER   NOT NULL DEFAULT 0,
	`authorid` INTEGER   NOT NULL DEFAULT 0,
	`postdatetime` DATETIME   NOT NULL DEFAULT CURRENT_DATE,
	`amount` INTEGER   NOT NULL DEFAULT 0,
	`netamount` INTEGER   NOT NULL DEFAULT 0
);

DROP TABLE IF EXISTS `dnt_albumcategories`;
CREATE TABLE `dnt_albumcategories` (
	`albumcateid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`title` CHAR(50)   NOT NULL ,
	`description` CHAR(300)    ,
	`albumcount` INTEGER   NOT NULL DEFAULT 0,
	`displayorder` INTEGER   NOT NULL DEFAULT 0
);

DROP TABLE IF EXISTS `dnt_online`;
CREATE TABLE `dnt_online` (
	`olid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`userid` INTEGER   NOT NULL DEFAULT (-1),
	`ip` VARCHAR(15)   NOT NULL DEFAULT '0.0.0.0',
	`username` VARCHAR(20)   NOT NULL DEFAULT '',
	`nickname` VARCHAR(20)   NOT NULL DEFAULT '',
	`password` CHAR(32)   NOT NULL DEFAULT '',
	`groupid` INTEGER   NOT NULL DEFAULT 0,
	`olimg` VARCHAR(80)   NOT NULL DEFAULT '',
	`adminid` INTEGER   NOT NULL DEFAULT 0,
	`invisible` INTEGER   NOT NULL DEFAULT 0,
	`action` INTEGER   NOT NULL DEFAULT 0,
	`lastactivity` INTEGER   NOT NULL DEFAULT 0,
	`lastposttime` CHAR(30)   NOT NULL DEFAULT '1970-01-01 00:00:00',
	`lastpostpmtime` CHAR(30)   NOT NULL DEFAULT '1970-01-01 00:00:00',
	`lastsearchtime` CHAR(30)   NOT NULL DEFAULT '1970-01-01 00:00:00',
	`lastupdatetime` CHAR(30)   NOT NULL DEFAULT CURRENT_DATE,
	`forumid` INTEGER   NOT NULL DEFAULT 0,
	`forumname` VARCHAR(50)   NOT NULL DEFAULT '',
	`titleid` INTEGER   NOT NULL DEFAULT 0,
	`title` VARCHAR(80)   NOT NULL DEFAULT '',
	`verifycode` VARCHAR(10)   NOT NULL DEFAULT '',
	`newpms` INTEGER   NOT NULL DEFAULT 0,
	`newnotices` INTEGER   NOT NULL DEFAULT 0
);

DROP TABLE IF EXISTS `dnt_goodsusercredits`;
CREATE TABLE `dnt_goodsusercredits` (
	`id` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`uid` INTEGER   NOT NULL DEFAULT 0,
	`oneweek` INTEGER   NOT NULL DEFAULT 0,
	`onemonth` INTEGER   NOT NULL DEFAULT 0,
	`sixmonth` INTEGER   NOT NULL DEFAULT 0,
	`sixmonthago` INTEGER   NOT NULL DEFAULT 0,
	`ratefrom` INTEGER   NOT NULL ,
	`ratetype` INTEGER   NOT NULL DEFAULT 0
);

DROP TABLE IF EXISTS `dnt_photos`;
CREATE TABLE `dnt_photos` (
	`photoid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`filename` CHAR(255)   NOT NULL ,
	`attachment` CHAR(255)   NOT NULL ,
	`filesize` INTEGER   NOT NULL ,
	`title` CHAR(20)   NOT NULL ,
	`description` CHAR(200)   NOT NULL DEFAULT '',
	`postdate` DATETIME   NOT NULL DEFAULT CURRENT_DATE,
	`albumid` INTEGER   NOT NULL ,
	`userid` INTEGER   NOT NULL ,
	`username` CHAR(20)   NOT NULL DEFAULT '',
	`views` INTEGER   NOT NULL DEFAULT 0,
	`commentstatus` INTEGER   NOT NULL DEFAULT 0,
	`tagstatus` INTEGER   NOT NULL DEFAULT 0,
	`comments` INTEGER   NOT NULL DEFAULT 0,
	`isattachment` INTEGER   NOT NULL DEFAULT 0,
	`width` INTEGER    ,
	`height` INTEGER    
);

DROP TABLE IF EXISTS `dnt_help`;
CREATE TABLE `dnt_help` (
	`id` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`title` VARCHAR(100)   NOT NULL ,
	`message` TEXT    ,
	`pid` INTEGER   NOT NULL ,
	`orderby` INTEGER    DEFAULT 0
);

DROP TABLE IF EXISTS `dnt_forumfields`;
CREATE TABLE `dnt_forumfields` (
	`fid` INTEGER   NOT NULL ,
	`password` VARCHAR(16)   NOT NULL ,
	`icon` VARCHAR(255)    ,
	`postcredits` VARCHAR(255)    ,
	`replycredits` VARCHAR(255)    ,
	`redirect` VARCHAR(255)    ,
	`attachextensions` VARCHAR(255)    ,
	`rules` TEXT    ,
	`topictypes` TEXT    ,
	`viewperm` TEXT    ,
	`postperm` TEXT    ,
	`replyperm` TEXT    ,
	`getattachperm` TEXT    ,
	`postattachperm` TEXT    ,
	`moderators` TEXT    ,
	`description` TEXT    ,
	`applytopictype` INTEGER   NOT NULL DEFAULT 0,
	`postbytopictype` INTEGER   NOT NULL DEFAULT 0,
	`viewbytopictype` INTEGER   NOT NULL DEFAULT 0,
	`topictypeprefix` INTEGER   NOT NULL DEFAULT 0,
	`permuserlist` TEXT    ,
	`seokeywords` TEXT    ,
	`seodescription` TEXT    ,
	`rewritename` VARCHAR(20)    
);

DROP TABLE IF EXISTS `dnt_tradeoptionvars`;
CREATE TABLE `dnt_tradeoptionvars` (
	`typeid` INTEGER   NOT NULL ,
	`pid` INTEGER    ,
	`optionid` INTEGER    ,
	`value` TEXT    
);

DROP TABLE IF EXISTS `dnt_locations`;
CREATE TABLE `dnt_locations` (
	`lid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`city` VARCHAR(50)   NOT NULL DEFAULT '',
	`state` VARCHAR(50)   NOT NULL DEFAULT '',
	`country` VARCHAR(50)   NOT NULL DEFAULT '',
	`zipcode` VARCHAR(20)   NOT NULL DEFAULT ''
);

DROP TABLE IF EXISTS `dnt_smilies`;
CREATE TABLE `dnt_smilies` (
	`id` INTEGER   NOT NULL ,
	`displayorder` INTEGER   NOT NULL ,
	`type` INTEGER   NOT NULL ,
	`code` VARCHAR(30)   NOT NULL ,
	`url` VARCHAR(60)   NOT NULL 
);

DROP TABLE IF EXISTS `dnt_shopthemes`;
CREATE TABLE `dnt_shopthemes` (
	`themeid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`directory` VARCHAR(100)   NOT NULL DEFAULT '',
	`name` VARCHAR(50)   NOT NULL DEFAULT '',
	`author` VARCHAR(100)   NOT NULL DEFAULT '',
	`createdate` VARCHAR(50)   NOT NULL DEFAULT '',
	`copyright` VARCHAR(100)   NOT NULL DEFAULT ''
);

DROP TABLE IF EXISTS `dnt_phototags`;
CREATE TABLE `dnt_phototags` (
	`tagid` INTEGER   NOT NULL DEFAULT 0,
	`photoid` INTEGER   NOT NULL DEFAULT 0
);

DROP TABLE IF EXISTS `dnt_orders`;
CREATE TABLE `dnt_orders` (
	`orderid` CHAR(10)   NOT NULL ,
	`status` CHAR(10)    ,
	`buyer` CHAR(10)    ,
	`admin` CHAR(10)    ,
	`uid` INTEGER    ,
	`amount` INTEGER    ,
	`price` FLOAT    ,
	`submitdate` INTEGER    ,
	`confirmdate` INTEGER    
);

DROP TABLE IF EXISTS `dnt_photocomments`;
CREATE TABLE `dnt_photocomments` (
	`commentid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`photoid` INTEGER   NOT NULL ,
	`username` VARCHAR(20)   NOT NULL ,
	`userid` INTEGER   NOT NULL ,
	`ip` VARCHAR(100)   NOT NULL DEFAULT '',
	`postdatetime` DATETIME   NOT NULL ,
	`content` VARCHAR(2000)   NOT NULL 
);

DROP TABLE IF EXISTS `dnt_shops`;
CREATE TABLE `dnt_shops` (
	`shopid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`logo` VARCHAR(100)   NOT NULL ,
	`shopname` VARCHAR(50)   NOT NULL DEFAULT '',
	`categoryid` INTEGER   NOT NULL DEFAULT 0,
	`themeid` INTEGER   NOT NULL DEFAULT 0,
	`themepath` CHAR(50)   NOT NULL DEFAULT '',
	`uid` INTEGER   NOT NULL DEFAULT 0,
	`username` CHAR(20)   NOT NULL DEFAULT '',
	`introduce` VARCHAR(500)   NOT NULL DEFAULT '',
	`lid` INTEGER   NOT NULL DEFAULT 0,
	`locus` CHAR(20)   NOT NULL ,
	`bulletin` VARCHAR(500)   NOT NULL DEFAULT '',
	`createdatetime` DATETIME   NOT NULL DEFAULT CURRENT_DATE,
	`invisible` INTEGER   NOT NULL ,
	`viewcount` INTEGER   NOT NULL DEFAULT 0
);

DROP TABLE IF EXISTS `dnt_topictags`;
CREATE TABLE `dnt_topictags` (
	`tagid` INTEGER   NOT NULL DEFAULT 0,
	`tid` INTEGER   NOT NULL DEFAULT 0
);

DROP TABLE IF EXISTS `dnt_goods`;
CREATE TABLE `dnt_goods` (
	`goodsid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`shopid` INTEGER   NOT NULL DEFAULT 0,
	`categoryid` INTEGER   NOT NULL DEFAULT 0,
	`parentcategorylist` CHAR(300)   NOT NULL DEFAULT '',
	`shopcategorylist` CHAR(300)   NOT NULL DEFAULT ',',
	`recommend` INTEGER   NOT NULL DEFAULT 0,
	`discount` INTEGER   NOT NULL DEFAULT 0,
	`selleruid` INTEGER   NOT NULL DEFAULT 0,
	`seller` CHAR(20)   NOT NULL DEFAULT '',
	`account` CHAR(50)   NOT NULL ,
	`title` CHAR(60)   NOT NULL ,
	`magic` INTEGER   NOT NULL DEFAULT 0,
	`price` NUMERIC   NOT NULL DEFAULT 0,
	`amount` INTEGER   NOT NULL DEFAULT 0,
	`quality` INTEGER   NOT NULL DEFAULT 1,
	`lid` INTEGER   NOT NULL ,
	`locus` CHAR(20)   NOT NULL ,
	`transport` INTEGER   NOT NULL DEFAULT 0,
	`ordinaryfee` NUMERIC   NOT NULL DEFAULT 0,
	`expressfee` NUMERIC   NOT NULL DEFAULT 0,
	`emsfee` NUMERIC   NOT NULL ,
	`itemtype` INTEGER   NOT NULL DEFAULT 0,
	`dateline` DATETIME   NOT NULL DEFAULT CURRENT_DATE,
	`expiration` DATETIME   NOT NULL ,
	`lastbuyer` CHAR(10)   NOT NULL DEFAULT '',
	`lasttrade` DATETIME   NOT NULL ,
	`lastupdate` DATETIME   NOT NULL DEFAULT CURRENT_DATE,
	`totalitems` INTEGER   NOT NULL ,
	`tradesum` NUMERIC   NOT NULL DEFAULT 0,
	`closed` INTEGER   NOT NULL DEFAULT 0,
	`aid` INTEGER   NOT NULL ,
	`goodspic` CHAR(100)   NOT NULL DEFAULT '',
	`displayorder` INTEGER   NOT NULL DEFAULT 0,
	`costprice` NUMERIC   NOT NULL DEFAULT 0,
	`invoice` INTEGER   NOT NULL DEFAULT 0,
	`repair` INTEGER   NOT NULL DEFAULT 0,
	`message` TEXT   NOT NULL ,
	`otherlink` CHAR(250)   NOT NULL ,
	`readperm` INTEGER   NOT NULL ,
	`tradetype` INTEGER   NOT NULL ,
	`viewcount` INTEGER   NOT NULL DEFAULT 0,
	`invisible` INTEGER   NOT NULL DEFAULT 0,
	`smileyoff` INTEGER   NOT NULL ,
	`bbcodeoff` INTEGER   NOT NULL DEFAULT 0,
	`parseurloff` INTEGER   NOT NULL ,
	`highlight` VARCHAR(500)   NOT NULL DEFAULT ''
);

DROP TABLE IF EXISTS `dnt_stats`;
CREATE TABLE `dnt_stats` (
	`type` CHAR(10)   NOT NULL ,
	`variable` CHAR(20)   NOT NULL ,
	`count` INTEGER   NOT NULL DEFAULT 0
);

DROP TABLE IF EXISTS `dnt_statvars`;
CREATE TABLE `dnt_statvars` (
	`type` CHAR(20)   NOT NULL ,
	`variable` CHAR(20)   NOT NULL ,
	`value` TEXT   NOT NULL 
);

DROP TABLE IF EXISTS `dnt_bonuslog`;
CREATE TABLE `dnt_bonuslog` (
	`tid` INTEGER   NOT NULL ,
	`authorid` INTEGER   NOT NULL ,
	`answerid` INTEGER   NOT NULL ,
	`answername` CHAR(20)   NOT NULL ,
	`pid` INTEGER   NOT NULL ,
	`dateline` DATETIME   NOT NULL ,
	`bonus` INTEGER   NOT NULL ,
	`extid` INTEGER   NOT NULL ,
	`isbest` INTEGER   NOT NULL 
);

DROP TABLE IF EXISTS `dnt_debatediggs`;
CREATE TABLE `dnt_debatediggs` (
	`tid` INTEGER   NOT NULL ,
	`pid` INTEGER   NOT NULL ,
	`digger` CHAR(20)   NOT NULL ,
	`diggerid` INTEGER   NOT NULL ,
	`diggerip` CHAR(15)   NOT NULL ,
	`diggdatetime` DATETIME   NOT NULL 
);

DROP TABLE IF EXISTS `dnt_tags`;
CREATE TABLE `dnt_tags` (
	`tagid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`tagname` CHAR(10)   NOT NULL ,
	`userid` INTEGER   NOT NULL DEFAULT 0,
	`postdatetime` DATETIME   NOT NULL ,
	`orderid` INTEGER   NOT NULL DEFAULT 0,
	`color` CHAR(6)   NOT NULL ,
	`count` INTEGER   NOT NULL DEFAULT 0,
	`fcount` INTEGER   NOT NULL DEFAULT 0,
	`pcount` INTEGER   NOT NULL DEFAULT 0,
	`scount` INTEGER   NOT NULL DEFAULT 0,
	`vcount` INTEGER   NOT NULL DEFAULT 0,
	`gcount` INTEGER   NOT NULL DEFAULT 0
);

DROP TABLE IF EXISTS `dnt_debates`;
CREATE TABLE `dnt_debates` (
	`tid` INTEGER PRIMARY KEY  NOT NULL ,
	`positiveopinion` VARCHAR(200)   NOT NULL ,
	`negativeopinion` VARCHAR(200)   NOT NULL ,
	`terminaltime` DATETIME   NOT NULL ,
	`positivediggs` INTEGER   NOT NULL DEFAULT 0,
	`negativediggs` INTEGER   NOT NULL DEFAULT 0
);

DROP TABLE IF EXISTS `dnt_notices`;
CREATE TABLE `dnt_notices` (
	`nid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`uid` INTEGER   NOT NULL ,
	`type` INTEGER   NOT NULL ,
	`new` INTEGER   NOT NULL ,
	`posterid` INTEGER   NOT NULL ,
	`poster` CHAR(20)   NOT NULL ,
	`note` TEXT   NOT NULL ,
	`postdatetime` DATETIME   NOT NULL 
);

DROP TABLE IF EXISTS `dnt_shoplinks`;
CREATE TABLE `dnt_shoplinks` (
	`id` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`shopid` INTEGER   NOT NULL DEFAULT 0,
	`displayorder` INTEGER   NOT NULL ,
	`name` VARCHAR(100)   NOT NULL ,
	`linkshopid` INTEGER   NOT NULL DEFAULT 0
);

DROP TABLE IF EXISTS `dnt_statistics`;
CREATE TABLE `dnt_statistics` (
	`totaltopic` INTEGER   NOT NULL ,
	`totalpost` INTEGER   NOT NULL ,
	`totalusers` INTEGER   NOT NULL ,
	`lastusername` CHAR(20)   NOT NULL ,
	`lastuserid` INTEGER   NOT NULL ,
	`highestonlineusercount` INTEGER    ,
	`highestonlineusertime` DATETIME    ,
	`yesterdayposts` INTEGER   NOT NULL DEFAULT 0,
	`highestposts` INTEGER   NOT NULL DEFAULT 0,
	`highestpostsdate` CHAR(10)   NOT NULL DEFAULT ''
);

DROP TABLE IF EXISTS `dnt_posts4`;
CREATE TABLE `dnt_posts4` (
	`pid` INTEGER PRIMARY KEY  NOT NULL DEFAULT 0,
	`fid` INTEGER   NOT NULL ,
	`tid` INTEGER   NOT NULL ,
	`parentid` INTEGER   NOT NULL DEFAULT 0,
	`layer` INTEGER   NOT NULL DEFAULT 0,
	`poster` VARCHAR(20)   NOT NULL DEFAULT '',
	`posterid` INTEGER   NOT NULL DEFAULT 0,
	`title` VARCHAR(80)   NOT NULL ,
	`postdatetime` DATETIME   NOT NULL DEFAULT CURRENT_DATE,
	`message` TEXT   NOT NULL DEFAULT '',
	`ip` VARCHAR(15)   NOT NULL DEFAULT '',
	`lastedit` VARCHAR(50)   NOT NULL DEFAULT '',
	`invisible` INTEGER   NOT NULL DEFAULT 0,
	`usesig` INTEGER   NOT NULL DEFAULT 0,
	`htmlon` INTEGER   NOT NULL DEFAULT 0,
	`smileyoff` INTEGER   NOT NULL DEFAULT 0,
	`parseurloff` INTEGER   NOT NULL DEFAULT 0,
	`bbcodeoff` INTEGER   NOT NULL DEFAULT 0,
	`attachment` INTEGER   NOT NULL DEFAULT 0,
	`rate` INTEGER   NOT NULL DEFAULT 0,
	`ratetimes` INTEGER   NOT NULL DEFAULT 0
);

DROP TABLE IF EXISTS `dnt_topics`;
CREATE TABLE `dnt_topics` (
	`tid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`fid` INTEGER   NOT NULL ,
	`iconid` INTEGER   NOT NULL DEFAULT 0,
	`readperm` INTEGER   NOT NULL DEFAULT 0,
	`price` INTEGER   NOT NULL DEFAULT 0,
	`poster` CHAR(20)   NOT NULL DEFAULT '',
	`posterid` INTEGER   NOT NULL DEFAULT 0,
	`title` CHAR(60)   NOT NULL ,
	`postdatetime` DATETIME   NOT NULL DEFAULT CURRENT_DATE,
	`lastpost` DATETIME   NOT NULL DEFAULT CURRENT_DATE,
	`lastpostid` INTEGER   NOT NULL DEFAULT 0,
	`lastposter` CHAR(20)   NOT NULL DEFAULT '',
	`lastposterid` INTEGER   NOT NULL DEFAULT 0,
	`views` INTEGER   NOT NULL DEFAULT 0,
	`replies` INTEGER   NOT NULL DEFAULT 0,
	`displayorder` INTEGER   NOT NULL DEFAULT 0,
	`highlight` VARCHAR(500)   NOT NULL DEFAULT '',
	`digest` INTEGER   NOT NULL DEFAULT 0,
	`hide` INTEGER   NOT NULL DEFAULT 0,
	`attachment` INTEGER   NOT NULL DEFAULT 0,
	`moderated` INTEGER   NOT NULL DEFAULT 0,
	`closed` INTEGER   NOT NULL DEFAULT 0,
	`magic` INTEGER   NOT NULL DEFAULT 0,
	`identify` INTEGER   NOT NULL DEFAULT '0',
	`special` INTEGER   NOT NULL DEFAULT 0,
	`typeid` INTEGER   NOT NULL DEFAULT 0,
	`rate` INTEGER   NOT NULL DEFAULT 0,
	`attention` INTEGER   NOT NULL DEFAULT 0
);

DROP TABLE IF EXISTS `dnt_shopcategories`;
CREATE TABLE `dnt_shopcategories` (
	`categoryid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`parentid` INTEGER   NOT NULL DEFAULT 0,
	`parentidlist` CHAR(300)   NOT NULL DEFAULT 0,
	`layer` INTEGER   NOT NULL DEFAULT 0,
	`childcount` INTEGER   NOT NULL DEFAULT 0,
	`syscategoryid` INTEGER   NOT NULL ,
	`name` CHAR(50)   NOT NULL DEFAULT '',
	`categorypic` VARCHAR(100)   NOT NULL DEFAULT '',
	`shopid` INTEGER   NOT NULL DEFAULT 0,
	`displayorder` INTEGER   NOT NULL DEFAULT 0
);

DROP TABLE IF EXISTS `dnt_myattachments`;
CREATE TABLE `dnt_myattachments` (
	`aid` INTEGER   NOT NULL ,
	`uid` INTEGER   NOT NULL ,
	`attachment` CHAR(100)   NOT NULL ,
	`description` CHAR(100)   NOT NULL ,
	`postdatetime` DATETIME   NOT NULL ,
	`downloads` INTEGER   NOT NULL ,
	`filename` CHAR(100)   NOT NULL ,
	`pid` INTEGER   NOT NULL DEFAULT 0,
	`tid` INTEGER   NOT NULL DEFAULT 0,
	`extname` VARCHAR(50)   NOT NULL DEFAULT ''
);

DROP TABLE IF EXISTS `dnt_postdebatefields`;
CREATE TABLE `dnt_postdebatefields` (
	`tid` INTEGER   NOT NULL DEFAULT 0,
	`pid` INTEGER   NOT NULL DEFAULT 0,
	`opinion` INTEGER   NOT NULL DEFAULT 0,
	`diggs` INTEGER   NOT NULL DEFAULT 0
);

DROP TABLE IF EXISTS `dnt_myposts`;
CREATE TABLE `dnt_myposts` (
	`uid` INTEGER   NOT NULL ,
	`tid` INTEGER   NOT NULL ,
	`pid` INTEGER   NOT NULL ,
	`dateline` DATETIME   NOT NULL 
);

DROP TABLE IF EXISTS `dnt_advertisements`;
CREATE TABLE `dnt_advertisements` (
	`advid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`available` INTEGER   NOT NULL ,
	`type` VARCHAR(50)   NOT NULL ,
	`displayorder` INTEGER   NOT NULL ,
	`title` VARCHAR(50)   NOT NULL ,
	`targets` VARCHAR(255)   NOT NULL ,
	`starttime` DATETIME   NOT NULL ,
	`endtime` DATETIME   NOT NULL ,
	`code` TEXT   NOT NULL DEFAULT '',
	`parameters` TEXT   NOT NULL DEFAULT ''
);

DROP TABLE IF EXISTS `dnt_scheduledevents`;
CREATE TABLE `dnt_scheduledevents` (
	`scheduleID` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`key` VARCHAR(50)   NOT NULL ,
	`lastexecuted` DATETIME   NOT NULL ,
	`servername` VARCHAR(100)   NOT NULL 
);

DROP TABLE IF EXISTS `dnt_posts2`;
CREATE TABLE `dnt_posts2` (
	`pid` INTEGER PRIMARY KEY  NOT NULL DEFAULT 0,
	`fid` INTEGER   NOT NULL ,
	`tid` INTEGER   NOT NULL ,
	`parentid` INTEGER   NOT NULL DEFAULT 0,
	`layer` INTEGER   NOT NULL DEFAULT 0,
	`poster` VARCHAR(20)   NOT NULL DEFAULT '',
	`posterid` INTEGER   NOT NULL DEFAULT 0,
	`title` VARCHAR(80)   NOT NULL ,
	`postdatetime` DATETIME   NOT NULL DEFAULT CURRENT_DATE,
	`message` TEXT   NOT NULL DEFAULT '',
	`ip` VARCHAR(15)   NOT NULL DEFAULT '',
	`lastedit` VARCHAR(50)   NOT NULL DEFAULT '',
	`invisible` INTEGER   NOT NULL DEFAULT 0,
	`usesig` INTEGER   NOT NULL DEFAULT 0,
	`htmlon` INTEGER   NOT NULL DEFAULT 0,
	`smileyoff` INTEGER   NOT NULL DEFAULT 0,
	`parseurloff` INTEGER   NOT NULL DEFAULT 0,
	`bbcodeoff` INTEGER   NOT NULL DEFAULT 0,
	`attachment` INTEGER   NOT NULL DEFAULT 0,
	`rate` INTEGER   NOT NULL DEFAULT 0,
	`ratetimes` INTEGER   NOT NULL DEFAULT 0
);

DROP TABLE IF EXISTS `dnt_spaceattachments`;
CREATE TABLE `dnt_spaceattachments` (
	`aid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`uid` INTEGER   NOT NULL DEFAULT 0,
	`spacepostid` INTEGER   NOT NULL DEFAULT 0,
	`postdatetime` DATETIME   NOT NULL DEFAULT CURRENT_DATE,
	`filename` CHAR(100)   NOT NULL DEFAULT '',
	`filetype` CHAR(50)   NOT NULL DEFAULT '',
	`filesize` INTEGER   NOT NULL DEFAULT 0,
	`attachment` CHAR(100)   NOT NULL DEFAULT '',
	`downloads` INTEGER   NOT NULL DEFAULT 0
);

DROP TABLE IF EXISTS `dnt_onlinetime`;
CREATE TABLE `dnt_onlinetime` (
	`uid` INTEGER PRIMARY KEY  NOT NULL ,
	`thismonth` INTEGER   NOT NULL DEFAULT 0,
	`total` INTEGER   NOT NULL DEFAULT 0,
	`lastupdate` DATETIME   NOT NULL DEFAULT CURRENT_DATE
);

DROP TABLE IF EXISTS `dnt_usergroups`;
CREATE TABLE `dnt_usergroups` (
	`groupid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`radminid` INTEGER   NOT NULL ,
	`type` INTEGER    DEFAULT 0,
	`system` INTEGER   NOT NULL DEFAULT '0',
	`grouptitle` VARCHAR(50)   NOT NULL ,
	`creditshigher` INTEGER   NOT NULL ,
	`creditslower` INTEGER    ,
	`stars` INTEGER   NOT NULL ,
	`color` CHAR(7)   NOT NULL ,
	`groupavatar` VARCHAR(60)   NOT NULL ,
	`readaccess` INTEGER   NOT NULL ,
	`allowvisit` INTEGER   NOT NULL DEFAULT 0,
	`allowpost` INTEGER   NOT NULL DEFAULT 0,
	`allowreply` INTEGER   NOT NULL DEFAULT 0,
	`allowpostpoll` INTEGER   NOT NULL DEFAULT 0,
	`allowdirectpost` INTEGER   NOT NULL DEFAULT 0,
	`allowgetattach` INTEGER   NOT NULL DEFAULT 0,
	`allowpostattach` INTEGER   NOT NULL DEFAULT 0,
	`allowvote` INTEGER   NOT NULL DEFAULT 0,
	`allowmultigroups` INTEGER   NOT NULL DEFAULT 0,
	`allowsearch` INTEGER   NOT NULL DEFAULT 0,
	`allowavatar` INTEGER   NOT NULL DEFAULT 0,
	`allowcstatus` INTEGER   NOT NULL DEFAULT 0,
	`allowuseblog` INTEGER   NOT NULL DEFAULT 0,
	`allowinvisible` INTEGER   NOT NULL DEFAULT 0,
	`allowtransfer` INTEGER   NOT NULL DEFAULT 0,
	`allowsetreadperm` INTEGER   NOT NULL DEFAULT 0,
	`allowsetattachperm` INTEGER   NOT NULL DEFAULT 0,
	`allowhidecode` INTEGER   NOT NULL DEFAULT 0,
	`allowhtml` INTEGER   NOT NULL DEFAULT 0,
	`allowcusbbcode` INTEGER   NOT NULL DEFAULT 0,
	`allownickname` INTEGER   NOT NULL DEFAULT 0,
	`allowsigbbcode` INTEGER   NOT NULL DEFAULT 0,
	`allowsigimgcode` INTEGER   NOT NULL DEFAULT 0,
	`allowviewpro` INTEGER   NOT NULL DEFAULT 0,
	`allowviewstats` INTEGER   NOT NULL DEFAULT 0,
	`disableperiodctrl` INTEGER   NOT NULL DEFAULT 0,
	`reasonpm` INTEGER   NOT NULL DEFAULT 0,
	`maxprice` INTEGER   NOT NULL ,
	`maxpmnum` INTEGER   NOT NULL ,
	`maxsigsize` INTEGER   NOT NULL ,
	`maxattachsize` INTEGER   NOT NULL ,
	`maxsizeperday` INTEGER   NOT NULL ,
	`attachextensions` CHAR(100)   NOT NULL ,
	`raterange` CHAR(500)   NOT NULL ,
	`allowspace` INTEGER   NOT NULL DEFAULT 0,
	`maxspaceattachsize` INTEGER   NOT NULL DEFAULT 0,
	`maxspacephotosize` INTEGER   NOT NULL DEFAULT 0,
	`allowdebate` INTEGER   NOT NULL DEFAULT 0,
	`allowbonus` INTEGER   NOT NULL DEFAULT 0,
	`minbonusprice` INTEGER   NOT NULL DEFAULT 0,
	`maxbonusprice` INTEGER   NOT NULL DEFAULT 0,
	`allowtrade` INTEGER   NOT NULL DEFAULT 0,
	`allowdiggs` INTEGER   NOT NULL DEFAULT 0
);

DROP TABLE IF EXISTS `dnt_posts3`;
CREATE TABLE `dnt_posts3` (
	`pid` INTEGER PRIMARY KEY  NOT NULL DEFAULT 0,
	`fid` INTEGER   NOT NULL ,
	`tid` INTEGER   NOT NULL ,
	`parentid` INTEGER   NOT NULL DEFAULT 0,
	`layer` INTEGER   NOT NULL DEFAULT 0,
	`poster` VARCHAR(20)   NOT NULL DEFAULT '',
	`posterid` INTEGER   NOT NULL DEFAULT 0,
	`title` VARCHAR(80)   NOT NULL ,
	`postdatetime` DATETIME   NOT NULL DEFAULT CURRENT_DATE,
	`message` TEXT   NOT NULL DEFAULT '',
	`ip` VARCHAR(15)   NOT NULL DEFAULT '',
	`lastedit` VARCHAR(50)   NOT NULL DEFAULT '',
	`invisible` INTEGER   NOT NULL DEFAULT 0,
	`usesig` INTEGER   NOT NULL DEFAULT 0,
	`htmlon` INTEGER   NOT NULL DEFAULT 0,
	`smileyoff` INTEGER   NOT NULL DEFAULT 0,
	`parseurloff` INTEGER   NOT NULL DEFAULT 0,
	`bbcodeoff` INTEGER   NOT NULL DEFAULT 0,
	`attachment` INTEGER   NOT NULL DEFAULT 0,
	`rate` INTEGER   NOT NULL DEFAULT 0,
	`ratetimes` INTEGER   NOT NULL DEFAULT 0
);

DROP TABLE IF EXISTS `dnt_favorites`;
CREATE TABLE `dnt_favorites` (
	`uid` INTEGER   NOT NULL ,
	`tid` INTEGER   NOT NULL ,
	`typeid` INTEGER   NOT NULL DEFAULT 0
);

DROP TABLE IF EXISTS `dnt_spacecategories`;
CREATE TABLE `dnt_spacecategories` (
	`categoryid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`title` VARCHAR(50)   NOT NULL DEFAULT '',
	`uid` INTEGER   NOT NULL DEFAULT 0,
	`description` VARCHAR(1000)   NOT NULL DEFAULT '',
	`typeid` INTEGER   NOT NULL DEFAULT 0,
	`categorycount` INTEGER   NOT NULL DEFAULT 0,
	`displayorder` INTEGER   NOT NULL DEFAULT 0
);

DROP TABLE IF EXISTS `dnt_goodsattachments`;
CREATE TABLE `dnt_goodsattachments` (
	`aid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`uid` INTEGER   NOT NULL DEFAULT 0,
	`goodsid` INTEGER   NOT NULL DEFAULT 0,
	`categoryid` INTEGER   NOT NULL DEFAULT 0,
	`postdatetime` DATETIME   NOT NULL DEFAULT CURRENT_DATE,
	`readperm` INTEGER   NOT NULL DEFAULT 0,
	`filename` CHAR(100)   NOT NULL DEFAULT '',
	`description` CHAR(100)   NOT NULL DEFAULT '',
	`filetype` CHAR(50)   NOT NULL DEFAULT '',
	`filesize` INTEGER   NOT NULL DEFAULT 0,
	`attachment` CHAR(100)   NOT NULL DEFAULT ''
);

DROP TABLE IF EXISTS `dnt_navs`;
CREATE TABLE `dnt_navs` (
	`id` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`parentid` INTEGER   NOT NULL DEFAULT 0,
	`name` CHAR(50)   NOT NULL ,
	`title` CHAR(255)   NOT NULL ,
	`url` CHAR(255)   NOT NULL ,
	`target` INTEGER   NOT NULL DEFAULT 0,
	`type` INTEGER   NOT NULL DEFAULT 0,
	`available` INTEGER   NOT NULL DEFAULT 0,
	`displayorder` INTEGER   NOT NULL ,
	`highlight` INTEGER   NOT NULL DEFAULT 0,
	`level` INTEGER   NOT NULL DEFAULT 0
);

DROP TABLE IF EXISTS `dnt_spacecomments`;
CREATE TABLE `dnt_spacecomments` (
	`commentid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`postid` INTEGER   NOT NULL DEFAULT 0,
	`author` VARCHAR(50)   NOT NULL DEFAULT '',
	`email` VARCHAR(100)   NOT NULL DEFAULT '',
	`url` VARCHAR(255)   NOT NULL DEFAULT '',
	`ip` VARCHAR(100)   NOT NULL DEFAULT '',
	`postdatetime` DATETIME   NOT NULL DEFAULT CURRENT_DATE,
	`content` VARCHAR(2000)   NOT NULL DEFAULT '',
	`parentid` INTEGER   NOT NULL ,
	`uid` INTEGER   NOT NULL ,
	`posttitle` VARCHAR(60)   NOT NULL 
);

DROP TABLE IF EXISTS `dnt_admingroups`;
CREATE TABLE `dnt_admingroups` (
	`admingid` INTEGER   NOT NULL ,
	`alloweditpost` INTEGER   NOT NULL ,
	`alloweditpoll` INTEGER   NOT NULL ,
	`allowstickthread` INTEGER   NOT NULL ,
	`allowmodpost` INTEGER   NOT NULL ,
	`allowdelpost` INTEGER   NOT NULL ,
	`allowmassprune` INTEGER   NOT NULL ,
	`allowrefund` INTEGER   NOT NULL ,
	`allowcensorword` INTEGER   NOT NULL ,
	`allowviewip` INTEGER   NOT NULL ,
	`allowbanip` INTEGER   NOT NULL ,
	`allowedituser` INTEGER   NOT NULL ,
	`allowmoduser` INTEGER   NOT NULL ,
	`allowbanuser` INTEGER   NOT NULL ,
	`allowpostannounce` INTEGER   NOT NULL ,
	`allowviewlog` INTEGER   NOT NULL ,
	`disablepostctrl` INTEGER   NOT NULL ,
	`allowviewrealname` INTEGER   NOT NULL 
);

DROP TABLE IF EXISTS `dnt_goodscategories`;
CREATE TABLE `dnt_goodscategories` (
	`categoryid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`parentid` INTEGER   NOT NULL DEFAULT 0,
	`layer` INTEGER   NOT NULL DEFAULT 0,
	`parentidlist` CHAR(300)   NOT NULL DEFAULT '',
	`displayorder` INTEGER   NOT NULL DEFAULT 0,
	`categoryname` CHAR(50)   NOT NULL DEFAULT '',
	`haschild` INTEGER   NOT NULL DEFAULT 0,
	`fid` INTEGER   NOT NULL DEFAULT 0,
	`pathlist` CHAR(3000)   NOT NULL DEFAULT '',
	`goodscount` INTEGER   NOT NULL DEFAULT 0
);

DROP TABLE IF EXISTS `dnt_adminvisitlog`;
CREATE TABLE `dnt_adminvisitlog` (
	`visitid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`uid` INTEGER    ,
	`username` VARCHAR(20)   NOT NULL ,
	`groupid` INTEGER    ,
	`grouptitle` VARCHAR(50)   NOT NULL ,
	`ip` VARCHAR(15)    ,
	`postdatetime` DATETIME    DEFAULT CURRENT_DATE,
	`actions` VARCHAR(100)   NOT NULL ,
	`others` VARCHAR(200)   NOT NULL 
);

DROP TABLE IF EXISTS `dnt_topictagcaches`;
CREATE TABLE `dnt_topictagcaches` (
	`tid` INTEGER   NOT NULL DEFAULT 0,
	`linktid` INTEGER   NOT NULL DEFAULT 0,
	`linktitle` CHAR(60)   NOT NULL 
);

DROP TABLE IF EXISTS `dnt_announcements`;
CREATE TABLE `dnt_announcements` (
	`id` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`poster` VARCHAR(20)   NOT NULL ,
	`posterid` INTEGER   NOT NULL ,
	`title` VARCHAR(250)   NOT NULL ,
	`displayorder` INTEGER   NOT NULL ,
	`starttime` DATETIME   NOT NULL ,
	`endtime` DATETIME   NOT NULL ,
	`message` TEXT   NOT NULL 
);

DROP TABLE IF EXISTS `dnt_attachments`;
CREATE TABLE `dnt_attachments` (
	`aid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`uid` INTEGER   NOT NULL DEFAULT 0,
	`tid` INTEGER   NOT NULL ,
	`pid` INTEGER   NOT NULL ,
	`postdatetime` DATETIME   NOT NULL ,
	`readperm` INTEGER   NOT NULL ,
	`filename` CHAR(100)   NOT NULL ,
	`description` CHAR(100)   NOT NULL ,
	`filetype` CHAR(50)   NOT NULL ,
	`filesize` INTEGER   NOT NULL ,
	`attachment` CHAR(100)   NOT NULL ,
	`downloads` INTEGER   NOT NULL ,
	`attachprice` INTEGER   NOT NULL DEFAULT 0
);

DROP TABLE IF EXISTS `dnt_spaceconfigs`;
CREATE TABLE `dnt_spaceconfigs` (
	`spaceid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`userid` INTEGER   NOT NULL ,
	`spacetitle` CHAR(100)   NOT NULL DEFAULT '',
	`description` CHAR(200)   NOT NULL DEFAULT '',
	`blogdispmode` INTEGER   NOT NULL DEFAULT 0,
	`bpp` INTEGER   NOT NULL DEFAULT 16,
	`commentpref` INTEGER   NOT NULL DEFAULT 0,
	`messagepref` INTEGER   NOT NULL DEFAULT 0,
	`rewritename` CHAR(100)   NOT NULL DEFAULT '',
	`themeid` INTEGER   NOT NULL DEFAULT 0,
	`themepath` CHAR(50)   NOT NULL DEFAULT '',
	`postcount` INTEGER   NOT NULL DEFAULT 0,
	`commentcount` INTEGER   NOT NULL DEFAULT 0,
	`visitedtimes` INTEGER   NOT NULL DEFAULT 0,
	`createdatetime` DATETIME   NOT NULL DEFAULT CURRENT_DATE,
	`updatedatetime` DATETIME   NOT NULL DEFAULT CURRENT_DATE,
	`defaulttab` INTEGER   NOT NULL DEFAULT 0,
	`status` INTEGER   NOT NULL DEFAULT 0
);

DROP TABLE IF EXISTS `dnt_attachtypes`;
CREATE TABLE `dnt_attachtypes` (
	`id` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`extension` VARCHAR(256)   NOT NULL ,
	`maxsize` INTEGER   NOT NULL 
);

DROP TABLE IF EXISTS `dnt_bbcodes`;
CREATE TABLE `dnt_bbcodes` (
	`id` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`available` INTEGER   NOT NULL ,
	`tag` VARCHAR(100)   NOT NULL ,
	`icon` VARCHAR(50)    ,
	`example` VARCHAR(255)   NOT NULL ,
	`params` INTEGER   NOT NULL ,
	`nest` INTEGER   NOT NULL ,
	`explanation` TEXT    ,
	`replacement` TEXT    ,
	`paramsdescript` TEXT    ,
	`paramsdefvalue` TEXT    
);

DROP TABLE IF EXISTS `dnt_mytopics`;
CREATE TABLE `dnt_mytopics` (
	`uid` INTEGER   NOT NULL ,
	`tid` INTEGER   NOT NULL ,
	`dateline` DATETIME   NOT NULL 
);

DROP TABLE IF EXISTS `dnt_creditslog`;
CREATE TABLE `dnt_creditslog` (
	`id` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`uid` INTEGER   NOT NULL ,
	`fromto` INTEGER   NOT NULL ,
	`sendcredits` INTEGER   NOT NULL ,
	`receivecredits` INTEGER   NOT NULL ,
	`send` FLOAT   NOT NULL ,
	`receive` FLOAT   NOT NULL ,
	`paydate` DATETIME   NOT NULL ,
	`operation` INTEGER   NOT NULL 
);

DROP TABLE IF EXISTS `dnt_words`;
CREATE TABLE `dnt_words` (
	`id` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`admin` VARCHAR(20)   NOT NULL ,
	`find` VARCHAR(255)   NOT NULL ,
	`replacement` VARCHAR(255)   NOT NULL 
);

DROP TABLE IF EXISTS `dnt_failedlogins`;
CREATE TABLE `dnt_failedlogins` (
	`ip` CHAR(15) PRIMARY KEY  NOT NULL ,
	`errcount` INTEGER   NOT NULL DEFAULT 0,
	`lastupdate` DATETIME   NOT NULL DEFAULT CURRENT_DATE
);

DROP TABLE IF EXISTS `dnt_posts5`;
CREATE TABLE `dnt_posts5` (
	`pid` INTEGER PRIMARY KEY  NOT NULL DEFAULT 0,
	`fid` INTEGER   NOT NULL ,
	`tid` INTEGER   NOT NULL ,
	`parentid` INTEGER   NOT NULL DEFAULT 0,
	`layer` INTEGER   NOT NULL DEFAULT 0,
	`poster` VARCHAR(20)   NOT NULL DEFAULT '',
	`posterid` INTEGER   NOT NULL DEFAULT 0,
	`title` VARCHAR(80)   NOT NULL ,
	`postdatetime` DATETIME   NOT NULL DEFAULT CURRENT_DATE,
	`message` TEXT   NOT NULL DEFAULT '',
	`ip` VARCHAR(15)   NOT NULL DEFAULT '',
	`lastedit` VARCHAR(50)   NOT NULL DEFAULT '',
	`invisible` INTEGER   NOT NULL DEFAULT 0,
	`usesig` INTEGER   NOT NULL DEFAULT 0,
	`htmlon` INTEGER   NOT NULL DEFAULT 0,
	`smileyoff` INTEGER   NOT NULL DEFAULT 0,
	`parseurloff` INTEGER   NOT NULL DEFAULT 0,
	`bbcodeoff` INTEGER   NOT NULL DEFAULT 0,
	`attachment` INTEGER   NOT NULL DEFAULT 0,
	`rate` INTEGER   NOT NULL DEFAULT 0,
	`ratetimes` INTEGER   NOT NULL DEFAULT 0
);

DROP TABLE IF EXISTS `dnt_topictypes`;
CREATE TABLE `dnt_topictypes` (
	`typeid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`displayorder` INTEGER   NOT NULL DEFAULT 0,
	`name` VARCHAR(30)   NOT NULL DEFAULT '',
	`description` VARCHAR(500)   NOT NULL DEFAULT ''
);

DROP TABLE IF EXISTS `dnt_tablelist`;
CREATE TABLE `dnt_tablelist` (
	`id` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`createdatetime` DATETIME   NOT NULL DEFAULT CURRENT_DATE,
	`description` VARCHAR(50)   NOT NULL DEFAULT '',
	`mintid` INTEGER   NOT NULL DEFAULT (-1),
	`maxtid` INTEGER   NOT NULL DEFAULT (-1)
);

DROP TABLE IF EXISTS `dnt_topicidentify`;
CREATE TABLE `dnt_topicidentify` (
	`identifyid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`name` VARCHAR(50)   NOT NULL ,
	`filename` VARCHAR(50)   NOT NULL 
);

DROP TABLE IF EXISTS `dnt_forumlinks`;
CREATE TABLE `dnt_forumlinks` (
	`id` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`displayorder` INTEGER   NOT NULL ,
	`name` VARCHAR(100)   NOT NULL ,
	`url` VARCHAR(100)   NOT NULL ,
	`note` VARCHAR(200)   NOT NULL ,
	`logo` VARCHAR(100)   NOT NULL 
);

DROP TABLE IF EXISTS `dnt_goodscreditrules`;
CREATE TABLE `dnt_goodscreditrules` (
	`id` INTEGER   NOT NULL ,
	`lowerlimit` INTEGER   NOT NULL DEFAULT 0,
	`upperlimit` INTEGER   NOT NULL DEFAULT 0,
	`sellericon` VARCHAR(20)   NOT NULL DEFAULT '',
	`buyericon` VARCHAR(20)   NOT NULL DEFAULT ''
);

DROP TABLE IF EXISTS `dnt_medals`;
CREATE TABLE `dnt_medals` (
	`medalid` INTEGER   NOT NULL ,
	`name` VARCHAR(50)   NOT NULL ,
	`available` INTEGER   NOT NULL DEFAULT 0,
	`image` VARCHAR(30)   NOT NULL DEFAULT ''
);

DROP TABLE IF EXISTS `dnt_medalslog`;
CREATE TABLE `dnt_medalslog` (
	`id` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`adminname` VARCHAR(50)    ,
	`adminid` INTEGER    ,
	`ip` VARCHAR(15)    ,
	`postdatetime` DATETIME    DEFAULT CURRENT_DATE,
	`username` VARCHAR(50)    ,
	`uid` INTEGER    ,
	`actions` VARCHAR(100)    ,
	`medals` INTEGER    ,
	`reason` VARCHAR(100)    
);

DROP TABLE IF EXISTS `dnt_moderatormanagelog`;
CREATE TABLE `dnt_moderatormanagelog` (
	`id` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`moderatoruid` INTEGER    ,
	`moderatorname` VARCHAR(50)    ,
	`groupid` INTEGER    ,
	`grouptitle` VARCHAR(50)    ,
	`ip` VARCHAR(15)    ,
	`postdatetime` DATETIME    DEFAULT CURRENT_DATE,
	`fid` INTEGER    ,
	`fname` VARCHAR(100)    ,
	`tid` INTEGER    ,
	`title` VARCHAR(200)    ,
	`actions` VARCHAR(50)    ,
	`reason` VARCHAR(200)    
);

DROP TABLE IF EXISTS `dnt_moderators`;
CREATE TABLE `dnt_moderators` (
	`uid` INTEGER   NOT NULL ,
	`fid` INTEGER   NOT NULL ,
	`displayorder` INTEGER   NOT NULL ,
	`inherited` INTEGER   NOT NULL 
);

DROP TABLE IF EXISTS `dnt_goodsleavewords`;
CREATE TABLE `dnt_goodsleavewords` (
	`id` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`goodsid` INTEGER   NOT NULL DEFAULT 0,
	`tradelogid` INTEGER   NOT NULL ,
	`isbuyer` INTEGER   NOT NULL DEFAULT 0,
	`uid` INTEGER   NOT NULL DEFAULT 0,
	`username` CHAR(20)   NOT NULL DEFAULT '',
	`message` CHAR(200)   NOT NULL ,
	`invisible` INTEGER   NOT NULL DEFAULT 0,
	`ip` VARCHAR(15)   NOT NULL DEFAULT '',
	`usesig` INTEGER   NOT NULL DEFAULT 0,
	`htmlon` INTEGER   NOT NULL ,
	`smileyoff` INTEGER   NOT NULL ,
	`parseurloff` INTEGER   NOT NULL ,
	`bbcodeoff` INTEGER   NOT NULL ,
	`postdatetime` DATETIME   NOT NULL DEFAULT CURRENT_DATE
);

DROP TABLE IF EXISTS `dnt_onlinelist`;
CREATE TABLE `dnt_onlinelist` (
	`groupid` INTEGER   NOT NULL ,
	`displayorder` INTEGER    ,
	`title` VARCHAR(50)   NOT NULL ,
	`img` VARCHAR(50)    
);

DROP TABLE IF EXISTS `dnt_paymentlog`;
CREATE TABLE `dnt_paymentlog` (
	`id` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`uid` INTEGER   NOT NULL ,
	`tid` INTEGER   NOT NULL ,
	`authorid` INTEGER   NOT NULL ,
	`buydate` DATETIME   NOT NULL DEFAULT CURRENT_DATE,
	`amount` INTEGER   NOT NULL ,
	`netamount` INTEGER   NOT NULL 
);

DROP TABLE IF EXISTS `dnt_userfields`;
CREATE TABLE `dnt_userfields` (
	`uid` INTEGER PRIMARY KEY  NOT NULL ,
	`website` VARCHAR(80)   NOT NULL DEFAULT '',
	`icq` VARCHAR(12)   NOT NULL DEFAULT '',
	`qq` VARCHAR(12)   NOT NULL DEFAULT '',
	`yahoo` VARCHAR(40)   NOT NULL DEFAULT '',
	`msn` VARCHAR(40)   NOT NULL DEFAULT '',
	`skype` VARCHAR(40)   NOT NULL DEFAULT '',
	`location` VARCHAR(50)   NOT NULL DEFAULT '',
	`customstatus` VARCHAR(50)   NOT NULL DEFAULT '',
	`avatar` VARCHAR(255)   NOT NULL DEFAULT 'avatars\common\0.gif',
	`avatarwidth` INTEGER   NOT NULL DEFAULT 60,
	`avatarheight` INTEGER   NOT NULL DEFAULT 60,
	`medals` VARCHAR(300)   NOT NULL DEFAULT '',
	`bio` VARCHAR(500)   NOT NULL DEFAULT '',
	`signature` VARCHAR(500)   NOT NULL DEFAULT '',
	`sightml` VARCHAR(1000)   NOT NULL DEFAULT '',
	`authstr` VARCHAR(20)   NOT NULL DEFAULT '',
	`authtime` DATETIME   NOT NULL DEFAULT CURRENT_DATE,
	`authflag` INTEGER   NOT NULL DEFAULT 0,
	`realname` VARCHAR(10)   NOT NULL DEFAULT '',
	`idcard` VARCHAR(20)   NOT NULL DEFAULT '',
	`mobile` VARCHAR(20)   NOT NULL DEFAULT '',
	`phone` VARCHAR(20)   NOT NULL DEFAULT '',
	`ignorepm` VARCHAR(1000)   NOT NULL DEFAULT ''
);

DROP TABLE IF EXISTS `dnt_pms`;
CREATE TABLE `dnt_pms` (
	`pmid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`msgfrom` VARCHAR(50)   NOT NULL ,
	`msgfromid` INTEGER   NOT NULL ,
	`msgto` VARCHAR(50)   NOT NULL ,
	`msgtoid` INTEGER   NOT NULL ,
	`folder` INTEGER   NOT NULL ,
	`new` INTEGER   NOT NULL ,
	`subject` VARCHAR(60)   NOT NULL ,
	`postdatetime` DATETIME   NOT NULL ,
	`message` TEXT   NOT NULL 
);

DROP TABLE IF EXISTS `dnt_postid`;
CREATE TABLE `dnt_postid` (
	`pid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`postdatetime` DATETIME   NOT NULL 
);

DROP TABLE IF EXISTS `dnt_spacecustomizepanels`;
CREATE TABLE `dnt_spacecustomizepanels` (
	`moduleid` INTEGER PRIMARY KEY  NOT NULL ,
	`userid` INTEGER NOT NULL ,
	`panelcontent` TEXT   NOT NULL DEFAULT ''
);

DROP TABLE IF EXISTS `dnt_posts1`;
CREATE TABLE `dnt_posts1` (
	`pid` INTEGER PRIMARY KEY  NOT NULL DEFAULT 0,
	`fid` INTEGER   NOT NULL ,
	`tid` INTEGER   NOT NULL ,
	`parentid` INTEGER   NOT NULL DEFAULT 0,
	`layer` INTEGER   NOT NULL DEFAULT 0,
	`poster` VARCHAR(20)   NOT NULL DEFAULT '',
	`posterid` INTEGER   NOT NULL DEFAULT 0,
	`title` VARCHAR(60)   NOT NULL ,
	`postdatetime` DATETIME   NOT NULL DEFAULT CURRENT_DATE,
	`message` TEXT   NOT NULL DEFAULT '',
	`ip` VARCHAR(15)   NOT NULL DEFAULT '',
	`lastedit` VARCHAR(50)   NOT NULL DEFAULT '',
	`invisible` INTEGER   NOT NULL DEFAULT 0,
	`usesig` INTEGER   NOT NULL DEFAULT 0,
	`htmlon` INTEGER   NOT NULL DEFAULT 0,
	`smileyoff` INTEGER   NOT NULL DEFAULT 0,
	`parseurloff` INTEGER   NOT NULL DEFAULT 0,
	`bbcodeoff` INTEGER   NOT NULL DEFAULT 0,
	`attachment` INTEGER   NOT NULL DEFAULT 0,
	`rate` INTEGER   NOT NULL DEFAULT 0,
	`ratetimes` INTEGER   NOT NULL DEFAULT 0
);

DROP TABLE IF EXISTS `dnt_ratelog`;
CREATE TABLE `dnt_ratelog` (
	`id` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`pid` INTEGER   NOT NULL ,
	`uid` INTEGER   NOT NULL ,
	`username` CHAR(20)   NOT NULL ,
	`extcredits` INTEGER   NOT NULL ,
	`postdatetime` DATETIME   NOT NULL DEFAULT CURRENT_DATE,
	`score` INTEGER   NOT NULL ,
	`reason` VARCHAR(50)   NOT NULL 
);

DROP TABLE IF EXISTS `dnt_searchcaches`;
CREATE TABLE `dnt_searchcaches` (
	`searchid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`keywords` VARCHAR(255)   NOT NULL ,
	`searchstring` VARCHAR(255)   NOT NULL ,
	`ip` VARCHAR(15)   NOT NULL ,
	`uid` INTEGER   NOT NULL ,
	`groupid` INTEGER   NOT NULL ,
	`postdatetime` DATETIME   NOT NULL ,
	`expiration` DATETIME   NOT NULL ,
	`topics` INTEGER   NOT NULL ,
	`tids` TEXT   NOT NULL 
);

DROP TABLE IF EXISTS `dnt_goodsrates`;
CREATE TABLE `dnt_goodsrates` (
	`id` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`goodstradelogid` INTEGER   NOT NULL DEFAULT 0,
	`message` CHAR(200)   NOT NULL DEFAULT '',
	`explain` CHAR(200)   NOT NULL DEFAULT '',
	`ip` VARCHAR(15)   NOT NULL DEFAULT '',
	`uid` INTEGER   NOT NULL DEFAULT 0,
	`uidtype` INTEGER   NOT NULL ,
	`username` CHAR(20)   NOT NULL DEFAULT '',
	`ratetouid` INTEGER   NOT NULL DEFAULT 0,
	`ratetousername` CHAR(20)   NOT NULL DEFAULT '',
	`postdatetime` DATETIME   NOT NULL DEFAULT CURRENT_DATE,
	`goodsid` INTEGER   NOT NULL ,
	`goodstitle` CHAR(60)   NOT NULL DEFAULT '',
	`price` NUMERIC   NOT NULL DEFAULT 0,
	`ratetype` INTEGER   NOT NULL DEFAULT 0
);

DROP TABLE IF EXISTS `dnt_spacelinks`;
CREATE TABLE `dnt_spacelinks` (
	`linkid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`userid` INTEGER   NOT NULL ,
	`linktitle` VARCHAR(50)   NOT NULL ,
	`linkurl` VARCHAR(255)   NOT NULL ,
	`description` VARCHAR(200)    
);

DROP TABLE IF EXISTS `dnt_polloptions`;
CREATE TABLE `dnt_polloptions` (
	`polloptionid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`tid` INTEGER   NOT NULL DEFAULT 0,
	`pollid` INTEGER   NOT NULL DEFAULT 0,
	`votes` INTEGER   NOT NULL DEFAULT 0,
	`displayorder` INTEGER   NOT NULL DEFAULT 0,
	`polloption` VARCHAR(80)   NOT NULL DEFAULT '',
	`voternames` TEXT   NOT NULL DEFAULT ''
);

DROP TABLE IF EXISTS `dnt_spacemoduledefs`;
CREATE TABLE `dnt_spacemoduledefs` (
	`moduledefid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`modulename` VARCHAR(20)   NOT NULL ,
	`cachetime` INTEGER   NOT NULL ,
	`configfile` VARCHAR(100)   NOT NULL ,
	`controller` VARCHAR(255)    
);

DROP TABLE IF EXISTS `dnt_spacemodules`;
CREATE TABLE `dnt_spacemodules` (
	`moduleid` INTEGER   NOT NULL ,
	`tabid` INTEGER   NOT NULL ,
	`uid` INTEGER   NOT NULL DEFAULT 0,
	`moduledefid` INTEGER   NOT NULL ,
	`panename` VARCHAR(10)   NOT NULL ,
	`displayorder` INTEGER   NOT NULL ,
	`userpref` VARCHAR(4000)    ,
	`val` INTEGER   NOT NULL ,
	`moduleurl` VARCHAR(255)   NOT NULL ,
	`moduletype` INTEGER   NOT NULL 
);

DROP TABLE IF EXISTS `dnt_spacepostcategories`;
CREATE TABLE `dnt_spacepostcategories` (
	`id` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`postid` INTEGER   NOT NULL DEFAULT 0,
	`categoryid` INTEGER   NOT NULL DEFAULT 0
);

DROP TABLE IF EXISTS `dnt_spaceposts`;
CREATE TABLE `dnt_spaceposts` (
	`postid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`author` VARCHAR(40)   NOT NULL ,
	`uid` INTEGER   NOT NULL ,
	`postdatetime` DATETIME   NOT NULL ,
	`content` TEXT   NOT NULL ,
	`title` VARCHAR(120)   NOT NULL ,
	`category` VARCHAR(255)   NOT NULL ,
	`poststatus` INTEGER   NOT NULL ,
	`commentstatus` INTEGER   NOT NULL ,
	`postupdatetime` DATETIME   NOT NULL ,
	`commentcount` INTEGER   NOT NULL ,
	`views` INTEGER   NOT NULL DEFAULT 0
);

DROP TABLE IF EXISTS `dnt_polls`;
CREATE TABLE `dnt_polls` (
	`pollid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`tid` INTEGER   NOT NULL DEFAULT 0,
	`displayorder` INTEGER   NOT NULL ,
	`multiple` INTEGER   NOT NULL DEFAULT 0,
	`visible` INTEGER   NOT NULL DEFAULT 0,
	`maxchoices` INTEGER   NOT NULL DEFAULT 0,
	`expiration` DATETIME   NOT NULL ,
	`uid` INTEGER   NOT NULL DEFAULT 0,
	`voternames` TEXT   NOT NULL DEFAULT ''
);

DROP TABLE IF EXISTS `dnt_goodstags`;
CREATE TABLE `dnt_goodstags` (
	`tagid` INTEGER   NOT NULL ,
	`goodsid` INTEGER   NOT NULL 
);

DROP TABLE IF EXISTS `dnt_spaceposttags`;
CREATE TABLE `dnt_spaceposttags` (
	`tagid` INTEGER   NOT NULL DEFAULT 0,
	`spacepostid` INTEGER   NOT NULL DEFAULT 0
);

DROP TABLE IF EXISTS `dnt_goodstradelogs`;
CREATE TABLE `dnt_goodstradelogs` (
	`id` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`goodsid` INTEGER   NOT NULL DEFAULT 0,
	`orderid` VARCHAR(50)   NOT NULL DEFAULT '',
	`tradeno` VARCHAR(50)   NOT NULL DEFAULT '',
	`subject` CHAR(60)   NOT NULL DEFAULT '',
	`price` NUMERIC   NOT NULL DEFAULT 0,
	`quality` INTEGER   NOT NULL ,
	`categoryid` INTEGER   NOT NULL DEFAULT 0,
	`number` INTEGER   NOT NULL DEFAULT 0,
	`tax` NUMERIC   NOT NULL ,
	`locus` VARCHAR(50)   NOT NULL DEFAULT '',
	`sellerid` INTEGER   NOT NULL DEFAULT 0,
	`seller` CHAR(20)   NOT NULL DEFAULT '',
	`selleraccount` VARCHAR(50)   NOT NULL ,
	`buyerid` INTEGER   NOT NULL DEFAULT 0,
	`buyer` CHAR(20)   NOT NULL DEFAULT '',
	`buyercontact` CHAR(100)   NOT NULL DEFAULT '',
	`buyercredit` INTEGER   NOT NULL DEFAULT 0,
	`buyermsg` CHAR(100)   NOT NULL DEFAULT '',
	`status` INTEGER   NOT NULL DEFAULT 0,
	`lastupdate` DATETIME   NOT NULL DEFAULT CURRENT_DATE,
	`offline` INTEGER   NOT NULL DEFAULT 0,
	`buyername` CHAR(20)   NOT NULL DEFAULT '',
	`buyerzip` VARCHAR(50)   NOT NULL DEFAULT '',
	`buyerphone` VARCHAR(50)   NOT NULL ,
	`buyermobile` VARCHAR(50)   NOT NULL DEFAULT '',
	`transport` INTEGER   NOT NULL DEFAULT 0,
	`transportfee` NUMERIC   NOT NULL DEFAULT 0,
	`transportpay` INTEGER   NOT NULL DEFAULT 1,
	`tradesum` NUMERIC   NOT NULL DEFAULT 0,
	`baseprice` NUMERIC   NOT NULL DEFAULT 0,
	`discount` INTEGER   NOT NULL DEFAULT 0,
	`ratestatus` INTEGER   NOT NULL DEFAULT 0,
	`message` TEXT   NOT NULL DEFAULT ''
);

DROP TABLE IF EXISTS `dnt_spacetabs`;
CREATE TABLE `dnt_spacetabs` (
	`tabid` INTEGER   NOT NULL ,
	`uid` INTEGER   NOT NULL ,
	`displayorder` INTEGER   NOT NULL ,
	`tabname` VARCHAR(50)   NOT NULL ,
	`iconfile` VARCHAR(50)    ,
	`template` VARCHAR(50)    
);

DROP TABLE IF EXISTS `dnt_spacethemes`;
CREATE TABLE `dnt_spacethemes` (
	`themeid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`directory` VARCHAR(100)   NOT NULL DEFAULT '',
	`name` VARCHAR(50)   NOT NULL DEFAULT '',
	`type` INTEGER   NOT NULL DEFAULT 0,
	`author` VARCHAR(100)   NOT NULL DEFAULT '',
	`createdate` VARCHAR(50)   NOT NULL DEFAULT '',
	`copyright` VARCHAR(100)   NOT NULL DEFAULT ''
);

DROP TABLE IF EXISTS `dnt_banned`;
CREATE TABLE `dnt_banned` (
	`id` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`ip1` INTEGER   NOT NULL ,
	`ip2` INTEGER   NOT NULL ,
	`ip3` INTEGER   NOT NULL ,
	`ip4` INTEGER   NOT NULL ,
	`admin` VARCHAR(50)   NOT NULL ,
	`dateline` DATETIME   NOT NULL ,
	`expiration` DATETIME   NOT NULL 
);

DROP TABLE IF EXISTS `dnt_users`;
CREATE TABLE `dnt_users` (
	`uid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`username` CHAR(20)   NOT NULL DEFAULT '',
	`nickname` CHAR(20)   NOT NULL DEFAULT '',
	`password` CHAR(32)   NOT NULL DEFAULT '',
	`secques` CHAR(8)   NOT NULL DEFAULT '',
	`spaceid` INTEGER   NOT NULL DEFAULT 0,
	`gender` INTEGER   NOT NULL DEFAULT 0,
	`adminid` INTEGER   NOT NULL DEFAULT 0,
	`groupid` INTEGER   NOT NULL DEFAULT 0,
	`groupexpiry` VARCHAR(50)   NOT NULL DEFAULT 0,
	`extgroupids` CHAR(60)   NOT NULL DEFAULT '',
	`regip` CHAR(15)   NOT NULL DEFAULT '',
	`joindate` DATETIME   NOT NULL DEFAULT CURRENT_DATE,
	`lastip` CHAR(15)   NOT NULL DEFAULT '',
	`lastvisit` DATETIME   NOT NULL DEFAULT CURRENT_DATE,
	`lastactivity` DATETIME   NOT NULL DEFAULT CURRENT_DATE,
	`lastpost` DATETIME   NOT NULL DEFAULT CURRENT_DATE,
	`lastpostid` INTEGER   NOT NULL DEFAULT 0,
	`lastposttitle` CHAR(60)   NOT NULL DEFAULT '',
	`posts` INTEGER   NOT NULL DEFAULT '0',
	`digestposts` INTEGER   NOT NULL DEFAULT '0',
	`oltime` INTEGER   NOT NULL DEFAULT '0',
	`pageviews` INTEGER   NOT NULL DEFAULT '0',
	`credits` INTEGER   NOT NULL DEFAULT '0',
	`extcredits1` NUMERIC   NOT NULL DEFAULT '0',
	`extcredits2` NUMERIC   NOT NULL DEFAULT '0',
	`extcredits3` NUMERIC   NOT NULL DEFAULT '0',
	`extcredits4` NUMERIC   NOT NULL DEFAULT '0',
	`extcredits5` NUMERIC   NOT NULL DEFAULT '0',
	`extcredits6` NUMERIC   NOT NULL DEFAULT '0',
	`extcredits7` NUMERIC   NOT NULL DEFAULT '0',
	`extcredits8` NUMERIC   NOT NULL DEFAULT '0',
	`avatarshowid` INTEGER   NOT NULL DEFAULT '0',
	`email` CHAR(50)   NOT NULL DEFAULT '',
	`bday` CHAR(10)   NOT NULL DEFAULT '',
	`sigstatus` INTEGER   NOT NULL DEFAULT '0',
	`tpp` INTEGER   NOT NULL DEFAULT '0',
	`ppp` INTEGER   NOT NULL DEFAULT '0',
	`templateid` INTEGER   NOT NULL DEFAULT '0',
	`pmsound` INTEGER   NOT NULL DEFAULT '0',
	`showemail` INTEGER   NOT NULL DEFAULT '0',
	`invisible` INTEGER   NOT NULL DEFAULT '0',
	`newpm` INTEGER   NOT NULL DEFAULT '0',
	`newpmcount` INTEGER   NOT NULL DEFAULT 0,
	`accessmasks` INTEGER   NOT NULL DEFAULT '0',
	`onlinestate` INTEGER   NOT NULL DEFAULT 0,
	`newsletter` INTEGER   NOT NULL DEFAULT '7'
);

DROP TABLE IF EXISTS `dnt_albums`;
CREATE TABLE `dnt_albums` (
	`albumid` INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL ,
	`albumcateid` INTEGER   NOT NULL ,
	`userid` INTEGER   NOT NULL DEFAULT (-1),
	`username` CHAR(20)   NOT NULL DEFAULT '',
	`title` CHAR(50)   NOT NULL DEFAULT '',
	`description` CHAR(200)   NOT NULL DEFAULT '',
	`logo` CHAR(255)   NOT NULL DEFAULT '',
	`password` CHAR(50)   NOT NULL DEFAULT '',
	`imgcount` INTEGER   NOT NULL DEFAULT 0,
	`views` INTEGER   NOT NULL DEFAULT 0,
	`type` INTEGER   NOT NULL DEFAULT 0,
	`createdatetime` DATETIME   NOT NULL DEFAULT CURRENT_DATE
);

