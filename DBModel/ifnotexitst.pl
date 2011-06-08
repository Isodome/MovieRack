#!/usr/bin/perl
use strict;

open IN,  "<", "createscript.sql" 		or die("Could not open createscript.sql");
open OUT, ">", "createscriptNew.sql" 	or die("Could not open creatscriptNew.sql");

my $line;
while($line = <IN>) {
	$line =~ s/CREATE\sINDEX(\s\W)/CREATE INDEX IF NOT EXISTS$1/i;
	$line =~ s/CREATE\sTABLE(\s\W)/CREATE TABLE IF NOT EXISTS$1/i;
	print OUT $line;	
}