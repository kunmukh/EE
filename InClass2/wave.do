onerror {resume}
quietly WaveActivateNextPane {} 0
add wave -noupdate -expand /InClass2/ain
add wave -noupdate -expand /InClass2/yout
add wave -noupdate /InClass2/enable
add wave -noupdate /InClass2/ain
add wave -noupdate /InClass2/enable
TreeUpdate [SetDefaultTree]
WaveRestoreCursors {{Cursor 1} {100 ps} 0}
quietly wave cursor active 1
configure wave -namecolwidth 150
configure wave -valuecolwidth 100
configure wave -justifyvalue left
configure wave -signalnamewidth 0
configure wave -snapdistance 10
configure wave -datasetprefix 0
configure wave -rowmargin 4
configure wave -childrowmargin 2
configure wave -gridoffset 0
configure wave -gridperiod 1
configure wave -griddelta 40
configure wave -timeline 0
configure wave -timelineunits ps
update
WaveRestoreZoom {0 ps} {200 ps}
view wave 
wave clipboard store
wave create -driver freeze -pattern counter -startvalue 00 -endvalue 11 -type Range -direction Up -period 50ps -step 1 -repeat forever -range 1 0 -starttime 0ps -endtime 1000ps sim:/InClass2/ain 
wave create -driver expectedOutput -pattern counter -startvalue 0000 -endvalue 1111 -type Range -direction Up -period 50ps -step 1 -repeat forever -range 3 0 -starttime 0ps -endtime 1000ps sim:/InClass2/yout 
wave create -driver freeze -pattern constant -value 1 -starttime 0ps -endtime 1000ps sim:/InClass2/enable 
wave create -driver freeze -pattern counter -startvalue 00 -endvalue 11 -type Range -direction Up -period 50ps -step 1 -repeat forever -range 1 0 -starttime 0ps -endtime 1000ps sim:/InClass2/ain 
WaveExpandAll -1
wave create -driver freeze -pattern constant -value 1 -starttime 0ps -endtime 1000ps sim:/InClass2/enable 
WaveCollapseAll -1
wave clipboard restore
