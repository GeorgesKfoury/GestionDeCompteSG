* Cette solution �vite la recalculation sauf si une nouvelle transcation est introduite via une API ou un le traitement 
d'un nouveau fichier csv en relancant l'application.
* J'ai suppos� que la valeur du compte journalier est celle apr�s avoir pris en compte les transactions faites au m�me jour
* Donc le fichier csv va �tre trait� durant le startup, et le compte ainsi que les transactions de d�bits par cat�gorie 
pr�calcul�es afin d'optimiser le temps de r�ponse au cas o� il y a un grand nombre de transactions. L'id�al est de 
persister ce calcul dans une BDD
* Une autre solution peut �tre envisag�e en utilisant une cache hybride avec R�dis dans le cas d'une architecture 
distribu� sous un Load Balancer, ce qui �vite un recalcul si les mises � jours des transactions sont fr�quents
