   M S B A M L   `   `   `  ����K  GClientApiAppDemo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:ClientApiAppDemoClientApiAppDemo  5   	   !    ClientApiAppDemo.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:ClientApiAppDemo     5   	     � Title$  Portföy��5   	   $	��470��6   $	��823��6&      Page.Background#���   62   .��5
      � $��5q�6    � ColumnDefinitions 5   
   �� 5      $
��151* �6   �� 5      $	��47* �6   .+�5   
   � $��2��6    5      �� 5      $	��47* �6#   �� 5      $
��151* �6#    � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      $��* �6    �� 5      $��* �6    �� 5      $
��Auto �6    .+�5      �� $��Hesap Secimi��6     Page.Foreground#���  6,   $��5,0,0,0q�6[   �� 5      $��0��6    �� DisplayMemberPath$ DisplayName��6R   $��0,3,0,5q�6�   $��1��6�   ��6$   �� *
Accounts+ �� Mode$ TwoWay=� �� SelectedItem 6r   �� *SelectedAccount+6�   &   System.Windows.Controls.DataGrid  5      $��1��6   $��2��6R      AutoGenerateColumns .     CanUserAddRows .  $	��120��6�   "   VerticalScrollBarVisibility$ Visible=�6�   ��6(   �� *FilteredPositions+	   Columns	 5      0  *System.Windows.Controls.DataGridTextColumn  5!      ,  &System.Windows.Controls.DataGridColumn
   Header$
 Hesap��6)   1  +System.Windows.Controls.DataGridBoundColumn   Binding 68   �� *	AccountId+6X     5"      $
 Sembol��6)    69   �� *Symbol+6W     5#      $
 Para Birimi��6)    6>   �� *
Currency+6]     5$      $
 Adet T2��6)    6:   �� *QtyT2+6W     5%      $
 Fiyat��6)    68   �� *LastPx+6U     5&      $
 Tutar��6)    68   �� *Amount+6U     5'      $
 Satılabilir Adet��6)    6C   �� *QtyAvailable+6f     5(      $
 Ort.Maliyet��6)    6>   �� *	AvgCost+6]     5)      $
 	Kar/Zarar��6)    6<   �� *PL+6V     5*      $
 % Kar/Zarar��6)    6>   �� *	PLPercent+6^   5-        50      $��2��6    . $��2��6�   $	��120��6�   $��0,5,0,0q�6�   ��6?   �� *FileteredOrders+ 6g   �� *SelectedOrderApiModel+ � ContextMenu 51      �� 52      .@�53      m� -   5        $��AddOrder��53   #   65   m� 54      -   5        $��	EditOrder��54   #   66   m� 55      -   5        $��CancelOrder��55   #   6Z   56      	 58      4  .System.Windows.Controls.DataGridTemplateColumn  59         CellTemplate 5:      �� 5;      .R�5<   "   �� -   5        .��5<   @   	Duzenle6I   5=      5?        5@       5A      �� 5B      .R�5C   "   �� -   5        .��5C   <   Sil6A   5D      5F      5S      �� 5W      -   5        $��3��5W      $��2��6"   $��0,5,0,0q�6W   $��5q�6h   $��	Yeni Emir��6t   5[      5\      5]      