   M S B A M L   `   `   `  ���� K  GClientApiAppDemo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:ClientApiAppDemoClientApiAppDemo       ClientApiAppDemo.OrderView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     0localclr-namespace:ClientApiAppDemo       � Title$  Emir Penceresi��$	��410��$	��400��   Page.Background#���   .��� .+��  J� RenderTransformOrigin$ 
0.49,0.506&�$��0,0,10,0q� � RowDefinitions �� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* ��� $��* � � ColumnDefinitions �� $��* ��� $��* �.+��� $��Hesap��
�������$��0��$��0��$��
15,15,0,15q��� $��0��$��1��$��	0,0,10,10q����� *SelectedAccount+�� $��Sembol��
�������$��1��$��0��$��
15,15,0,15q��� $��1��$��1������ *Symbol+y��� *SymbolLabelVisiblirVisibility+�� $��1��$��1��$��	0,0,10,10q����� *Symbol+y��� *SymbolBoxVisibility+�� $
��Adet��
�������$��2��$��0��$��
15,15,0,15q��� $��2��$��1��$��	0,0,10,10q����� *Volume+�� $��Fiyat��
�������$��3��$��0��$��
15,15,0,15q��� $��3��$��1��$��	0,0,10,10q����� *Price+�� $��	Emir Turu��
�������$��4��$��0��$��
15,15,0,15q�y��� *IsEditVisibility+�� -   $��OrderTypeCombo��$��4��$��1��$��	0,0,10,10q� �� SelectedItem �� *SelectedOrderType+y��� *IsEditVisibility+.@��� .��LMT�� .��PYS�� .��PLM�� $��Gecerlilik Suresi��
�������$��5��$��0��$��
15,15,0,15q�y��� *IsEditVisibility+�� -   $��ValidityTypeCombo��$��5��$��1��$��	0,0,10,10q� �� *SelectedValidityType+y��� *IsEditVisibility+.@��� .��GUN�� .��KIE�� .��TAR�� $��
Alis-Satis��$��6��$��0��
�������$��
15,15,0,15q�y��� *IsEditVisibility+�� $��6��$��1��$��	0,0,10,10q�y��� *IsEditVisibility+.+�	�  	� 	GroupName$
 Mode��$��0,0,48,0q�$
��Alis�� d� 	IsChecked �� *IsBuy+	� $
 Mode��$��Satis���� -   $��7��$��	0,0,10,10q�$��1��$��GÖNDER��