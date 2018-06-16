-- phpMyAdmin SQL Dump
-- version 4.7.4
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jun 16, 2018 at 07:04 AM
-- Server version: 10.1.28-MariaDB
-- PHP Version: 7.1.11

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `platform_db`
--

-- --------------------------------------------------------

--
-- Table structure for table `bm_gallery`
--

CREATE TABLE `bm_gallery` (
  `Id` int(11) NOT NULL,
  `Name` varchar(100) DEFAULT NULL,
  `Description` longtext,
  `ThumbnailImage` longblob NOT NULL,
  `DetailImage` longblob NOT NULL,
  `DownloadableImage` longblob NOT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `CreatedBy` varchar(45) DEFAULT NULL,
  `UpdatedDate` datetime DEFAULT NULL,
  `UpdatedBy` varchar(45) DEFAULT NULL,
  `IsActive` bit(1) DEFAULT b'1',
  `Version` varchar(45) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf16;

-- --------------------------------------------------------

--
-- Table structure for table `sys_file_system`
--

CREATE TABLE `sys_file_system` (
  `Id` int(11) NOT NULL,
  `FileName` varchar(100) NOT NULL,
  `FileDescription` varchar(100) DEFAULT NULL,
  `OriginalFileName` varchar(100) NOT NULL,
  `FileExtension` varchar(10) DEFAULT NULL,
  `FileContent` longtext,
  `ApplicationId` varchar(100) DEFAULT NULL,
  `Reference1` varchar(100) DEFAULT NULL,
  `Reference2` varchar(100) DEFAULT NULL,
  `Reference3` varchar(100) DEFAULT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `CreatedBy` varchar(45) DEFAULT NULL,
  `UpdatedDate` datetime DEFAULT NULL,
  `UpdatedBy` varchar(45) DEFAULT NULL,
  `IsActive` bit(1) DEFAULT b'1',
  `Version` varchar(45) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf16;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `bm_gallery`
--
ALTER TABLE `bm_gallery`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `sys_file_system`
--
ALTER TABLE `sys_file_system`
  ADD PRIMARY KEY (`Id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `bm_gallery`
--
ALTER TABLE `bm_gallery`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT for table `sys_file_system`
--
ALTER TABLE `sys_file_system`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
