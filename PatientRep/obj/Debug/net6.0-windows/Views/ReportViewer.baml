   M S B A M L   `   `   `  ????E  APatientRep, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:PatientRep.ViewsPatientRep.Views  5   	   #    PatientRep.Views.ReportViewer   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:PatientRep.Views     5   	     ? Title$  Report Viewer??5   	   $	??450??6   $	??800??6,    J? InputBindings 5	      ?? 5
   
   
 ?? Key$	 Esc??6>    ?? Command 6   ?? *OnCloseButtonPressed+6>   .??5      ?  ? ColumnDefinitions 5   
   ?? 5      $??5 ?6   ?? 5      ?? 5      $??5 ?6    ? RowDefinitions 5   
   ?? 5      $??40 ?6   ?? 5      $??5 ?6   ?? 5      ?? 5      $??5 ?6   ?? 5      $??40 ?6   .+?5   
   ?? $??5??6    
  Border#??[  6%   ?? 5   
   $??5??6   #??[  6%   $??2??6E   ?? 5   
   $??3??6   $??1??6(   #??[  65   ?? 5   
   $??3??6   $??3??6(   #??[  65   ?? 5!   
   $??2q?6   
??? ? ?$??1??69   $??2??6I   $??10q?6V    ?? CornerRadius$ 10??6b   ._?5"      ?? $??3q?6   $??Stretch=?5#      $??Visible=?6<   ??5"   "   Z? ??69   ?? *	Reports+ ?? UpdateSourceTrigger$ PropertyChanged=?5#   <   5$      ? 5&   
   -   5         ?Name$ buttons??5&      $??4??6    $??1??6-    5'      ?? 5(      ?? 5)      $	??120 ?6#   ?? 5*      $??50 ?6#   .+?5,      ?? $??Закрити??6     Buttons#??[ 5-      $??20(?66   $??1??6D   $??3q?6T   ??5.      ?? *OnCloseButtonPressed+50      51      ? 53   
   $??1??6    54      ?? 55      $??20 ?6#   ?? 56      $	??120 ?6#   ?? 57      .+?59      ?? $??1??6   $??Експорт??6%   $??20(?67     Buttons2#??[ 6E   $??5q?6g   ??5:      ?? *OnExportButtonPressed+5;      5=      5>      