   M S B A M L   `   `   `  ���� K  GClientApiAppDemo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:ClientApiAppDemoClientApiAppDemo  !    ClientApiAppDemo.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     0localclr-namespace:ClientApiAppDemo       � Title$  Portföy��$	��470��$	��823��   Page.Background#���   .��� $��5q� � ColumnDefinitions �� $
��151* ��� $	��47* �.+�� $��2�� �� $	��47* ��� $
��151* � � RowDefinitions �� $
��Auto ��� $��* ��� $��* ��� $
��Auto �.+��� $��Hesap Secimi��  Page.Foreground#���  $��5,0,0,0q��� $��0�� �� DisplayMemberPath$ DisplayName��$��0,3,0,5q�$��1������ *
Accounts+ �� Mode$ TwoWay=� �� SelectedItem �� *SelectedAccount+&   System.Windows.Controls.DataGrid  $��1��$��2��   AutoGenerateColumns .     CanUserAddRows .  $	��120��"   VerticalScrollBarVisibility$ Visible=����� *FilteredPositions+	   Columns	 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn
   Header$
 Hesap��1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *	AccountId+  $
 Sembol�� �� *Symbol+  $
 Para Birimi�� �� *
Currency+  $
 Adet T2�� �� *QtyT2+  $
 Fiyat�� �� *LastPx+  $
 Tutar�� �� *Amount+  $
 Satılabilir Adet�� �� *QtyAvailable+  $
 Ort.Maliyet�� �� *	AvgCost+  $
 	Kar/Zarar�� �� *PL+  $
 % Kar/Zarar�� �� *	PLPercent+  $��2�� . $��2��$	��120��$��0,5,0,0q����� *FileteredOrders+ �� *SelectedOrderApiModel+ � ContextMenu �� .@�m� -   $��AddOrder��m� -   $��	EditOrder��m� -   $��CancelOrder��	 4  .System.Windows.Controls.DataGridTemplateColumn     CellTemplate �� .R��� -   .��	Duzenle   �� .R��� -   .��Sil�� -   $��3��$��2��$��0,5,0,0q�$��5q�$��	Yeni Emir��