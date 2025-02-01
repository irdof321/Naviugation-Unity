# Analyse des Algorithmes de Recherche et de leurs Complexités

Ce document résume l'analyse et la comparaison de plusieurs algorithmes de recherche, notamment :

- **Dijkstra**  
- **Greedy Best-First Search**  
- **A\***

## Dijkstra

L'algorithme de Dijkstra permet de trouver le chemin le plus court depuis un nœud de départ vers tous les autres nœuds d'un graphe pondéré.

### Points Clés

- **Principe de fonctionnement :**  
  Pour chaque nœud, l’algorithme calcule la distance minimale accumulée depuis le départ.  
  À chaque itération, le nœud non visité avec la plus petite distance est sélectionné pour l'expansion.

- **Complexité (version naïve) :**  
  - **Sélection du nœud minimum :** \(O(N^2)\)  
  - **Relaxation des arêtes :** \(O(N \times v)\) (où \(v\) est le nombre maximum de voisins par nœud)  
  - **Complexité globale naïve :** \(O(N^2 + N \times v)\)  
  - Pour un graphe dense (où \(v \approx N\)), cela revient à \(O(N^2)\).

- **Optimisation avec une file de priorité (tas) :**  
  La complexité passe à :  
  \[
  O((N + E) \log N)
  \]
  avec \(E\) représentant le nombre total d'arêtes.

## Greedy Best-First Search

Le Greedy Best-First Search est un algorithme de recherche informée qui se base uniquement sur une fonction heuristique pour guider l'exploration vers l'objectif.

### Points Clés

- **Utilisation de l'heuristique :**  
  Chaque nœud reçoit une valeur \(h(n)\) qui estime la distance jusqu'à l'objectif.

- **Principe "avide" :**  
  À chaque itération, l'algorithme sélectionne le nœud ayant la plus faible valeur heuristique, sans prendre en compte le coût accumulé.

- **Complexité (analyse en insertion dans un heap) :**  
  - Calcul de l’heuristique pour chaque voisin : \(O(N \times v)\).  
  - Insertion dans le tas : \(O(N \times v \times \log N)\) dans le pire cas.  
  - En combinant ces deux contributions, on obtient une complexité de l’ordre de :  
    \[
    O(N \times v \times (1+\log N))
    \]
  - Pour \(N\) grand, on simplifie souvent en \(O(N \times v \times \log N)\) puisque \(\log N > 1\).

## A\*

A\* combine les approches de Dijkstra et du Greedy Best-First Search en utilisant une fonction de coût globale :

\[
f(n) = g(n) + h(n)
\]

### Points Clés

- **\(g(n)\) :**  
  Coût réel accumulé pour atteindre le nœud \(n\) depuis le départ.

- **\(h(n)\) :**  
  Heuristique estimant le coût restant pour atteindre l'objectif.

- **Principe de fonctionnement :**  
  - On démarre avec le nœud de départ et on initialise \(f(\text{départ}) = h(\text{départ})\).
  - À chaque itération, le nœud avec le plus faible \(f(n)\) est extrait de la file de priorité.
  - Les voisins du nœud courant sont alors examinés et mis à jour si un chemin moins coûteux est trouvé.
  - L'algorithme s'arrête lorsqu'il atteint le nœud objectif.

- **Conditions pour l'optimalité :**  
  Si l’heuristique \(h(n)\) est **admissible** (elle ne surestime jamais le coût réel) et **consistante**, A\* est garanti de trouver le chemin optimal.

## Conclusion

- **Dijkstra** est très efficace pour les graphes pondérés sans heuristique, surtout lorsqu'on utilise une file de priorité pour améliorer la sélection du nœud minimum.
- **Greedy Best-First Search** se concentre sur l'heuristique pour explorer rapidement vers l'objectif, mais ne garantit pas le chemin optimal.
- **A\*** combine les avantages des deux approches, guidé par une fonction \(f(n) = g(n) + h(n)\), permettant de trouver un chemin optimal sous les bonnes conditions sur l'heuristique.

Ce résumé vous permet de comparer les différents algorithmes de recherche et leurs implications en termes de complexité et d'efficacité. N'hésitez pas à approfondir chaque section pour mieux comprendre les mécanismes sous-jacents à chaque méthode.
