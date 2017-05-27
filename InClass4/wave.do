onerror {resume}
quietly WaveActivateNextPane {} 0
add wave -noupdate /InClass4/X
add wave -noupdate /InClass4/CLK
add wave -noupdate /InClass4/Y
add wave -noupdate /InClass4/X
add wave -noupdate /InClass4/CLK
TreeUpdate [SetDefaultTree]
WaveRestoreCursors {{Cursor 1} {0 ps} 0}
quietly wave cursor active 0
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
WaveRestoreZoom {50 ps} {1050 ps}
view wave 
wave clipboard store
wave create -driver freeze -pattern random -initialvalue HiZ -period 50ps -random_type Normal -seed 5 -starttime 0ps -endtime 1000ps sim:/InClass4/X 
wave create -driver freeze -pattern counter -startvalue 0 -endvalue 1 -type Range -direction Up -period 50ps -step 1 -repeat forever -starttime 0ps -endtime 1000ps sim:/InClass4/CLK 
WaveCollapseAll -1
wave clipboard restore
