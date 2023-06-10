CREATE SCHEMA `queryable` ;

CREATE TABLE `queryable4columns` (
   `Id` int(11) NOT NULL AUTO_INCREMENT,
   `Col2` varchar(45) NOT NULL,
   `Col3` datetime NOT NULL,
   `Col4` double NOT NULL,
   PRIMARY KEY (`Id`)
 ) ENGINE=MyISAM DEFAULT CHARSET=latin1;