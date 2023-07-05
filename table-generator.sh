location=./Manual/HTML/img/Abilities
declare -i entriesPerPage=11
declare -i page=2
declare -i counter=0

for entry in "$location"/*;
do
	echo "<tr>"
	echo "    <td><img class='ability-icon' src=\"./img/Abilities/$(basename "$entry")\"/></td>"
	v=$(basename "$entry")
	echo "    <td>${v%.*}</td>" | tr ";" ":"
	echo "</tr>"
	if [ "$counter" -ge "$entriesPerPage" ];
	then
		counter=0
		printf "</table></div><div class=\"page-footer relative-footer\">%d</div></div>" $page
		((page++))
        printf "<div class=\"page page-bg-%02d\">" $page
        echo '<div class="page-header">'
        echo '<span class="page-header-doc-title">Keep Talking and Nobody Explodes Mod</span>'
        echo '<span class="page-header-section-title">A: Abilities</span>'
        echo '</div><div class="page-content"><table class="abilities-table">'
	else
		((counter++))
	fi
done
echo "</table>"
echo ""
echo "Pages: $page" 
		
$SHELL