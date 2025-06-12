* Cette solution évite la recalculation sauf si une nouvelle transcation est introduite via une API ou un le traitement 
d'un nouveau fichier csv en relancant l'application.
* J'ai supposé que la valeur du compte journalier est celle après avoir pris en compte les transactions faites au même jour
* Donc le fichier csv va être traité durant le startup, et le compte ainsi que les transactions de débits par catégorie 
précalculées afin d'optimiser le temps de réponse au cas où il y a un grand nombre de transactions. L'idéal est de 
persister ce calcul dans une BDD
* Une autre solution peut être envisagée en utilisant une cache hybride avec Rédis dans le cas d'une architecture 
distribué sous un Load Balancer, ce qui évite un recalcul si les mises à jours des transactions sont fréquents
