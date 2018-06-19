-- phpMyAdmin SQL Dump
-- version 4.7.4
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jun 19, 2018 at 06:15 PM
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
-- Table structure for table `bm_user`
--

CREATE TABLE `bm_user` (
  `Id` int(11) NOT NULL,
  `Username` varchar(100) NOT NULL,
  `Password` longtext NOT NULL,
  `IsActive` bit(1) NOT NULL,
  `CreatedDate` datetime NOT NULL,
  `CreatedBy` varchar(100) NOT NULL,
  `UpdatedDate` datetime NOT NULL,
  `UpdatedBy` varchar(100) NOT NULL,
  `Version` varchar(100) DEFAULT NULL,
  `Email` varchar(100) NOT NULL,
  `SiteAdmin` bit(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf16;

--
-- Dumping data for table `bm_user`
--

INSERT INTO `bm_user` (`Id`, `Username`, `Password`, `IsActive`, `CreatedDate`, `CreatedBy`, `UpdatedDate`, `UpdatedBy`, `Version`, `Email`, `SiteAdmin`) VALUES
(1, 'admin', 'a3df12b8-7f64-41cb-9e85-61285a9b6375YWRtaW4=', b'1', '2018-06-19 00:00:00', 'system', '2018-06-19 00:00:00', 'system', NULL, 'admin@budgetmetal.com', b'1');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `bm_user`
--
ALTER TABLE `bm_user`
  ADD PRIMARY KEY (`Id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `bm_user`
--
ALTER TABLE `bm_user`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
