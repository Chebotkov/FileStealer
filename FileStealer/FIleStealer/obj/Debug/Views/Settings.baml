   M S B A M L   `   `   `  ����F  BFIleStealer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:FIleStealer.ViewModelsFIleStealer.ViewModels  5   	        FIleStealer.Views.Settings   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   6local$clr-namespace:FIleStealer.ViewModels     5   	   -   5          � Title$  Settings��5   	   $	��400��6   $	��500��6'    � WindowStartupLocation$ CenterScreen=�63    � DataContext 5	      '   !FIleStealer.ViewModels.SettingsVM  5
   
   6   .��5      �  � RowDefinitions 5   
   �� 5      $
��0.7* �6   6,   �� 5      $
��0.4* �6   6,    � ColumnDefinitions 5   
   �� 5      $��0.45* �6   6/   �� 5      $
��0.2* �6   6.   �� 5      $��0.45* �6   6/   .+�5   
   �� $��0��6   $��2��6    $��Setted Extensions��5   A   .��5      �  5      �� 5      $
��0.5* �6$   64   �� 5      $
��0.3* �6$   64   �� 5      $
��0.2* �6$   64   �� 5      $
��0.3* �6$   64    5       �� 5!      6)   �� 5"      6)   .+�5$      �� $��0��6   $��2��6,   .��5%      �� -   5        $��UsersExtensions��5%   d   ��6   �� *
Extensions+ �� SelectedItem 6A   �� *	Extension+ � ToolTip 5&      _� 5'      .��6&   !Extension must started with "."6G   5)      5*      �� 5,      $��1��6   $��1��6&   $��	10,5,10,5q�66   ��6I   �� *RemoveCommand+��6k   �� *	Extension+.��6�   Remove6�   �� 5-      $��2��6   $��2��6%   $��0,0,0,26q�6q   $��2��6�    �� Target 69   ��  �� ElementName$ TextblockForNewExtension��.��6�   Add new extension:6�   �� 5.      -   5        $��TextBoxForNewExtension��5.   7   $��3��6   $��0��6'   $��	0,10,0,10q�6U    6h   �� *NewExtension+	 �� Mode$	 TwoWay=�
 �� UpdateSourceTrigger$
 PropertyChanged=�.��6�    5/      _� 50      .��6"   !Extension must started with "."6C   .��51   '   52      �� 53      $��3��6   $��1��6&   $��	10,5,10,5q�66   ��6I   �� *
AddCommand+��6h   �� $ TextBoxForNewExtension�� �� Path$
 Text�.��6�   Add6�   54      55      �� 57   
   $��0��6   $��1��6   $��
5,60,5,151q�6.    J� RenderTransformOrigin$ 	0.5,1.114&�6B   ��6d   �� *AddFromListCommand+.��6�   Add6�   �� 58   
   $��0��6   $��0��6    $��	0 0 0 100q�60   $��Common extensions��5;      .��5=      �� .��5>      �� ��6   �� *CommonExtensions+ 6C   �� *SelectedCommonExtension+6v   5?      5@      �� 5B   
   $��1��6   $��0��6    $��0 0 0 5q�60   $��Select type of drives��5E      .��5G      �� .+�5H      	� -   5        $��RadioAll��5H      $	��2 3q�6.   .��6<   
All drives6H   	� 5I      -   5        $��
RadioReady��5I      $	��2 3q�60   .��6=   Ready drives6K   	� 5J      -   5        $��RadioRaU��5J      $	��2 3q�6.    d� 	IsChecked$
 TrueY�6;   .��6L   Ready and unremovable6c   	� 5K      -   5        $��RadioRO��5K      $	��2 3q�6-   .��6:   Removable only6J   	� 5L      -   5        $��RadioUO��5L      $	��2 3q�6-   .��6:   Unremovable only6L   5M      5N      5O      5P      