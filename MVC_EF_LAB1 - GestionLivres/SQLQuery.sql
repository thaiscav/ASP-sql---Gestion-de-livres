SELECT DATEDIFF(DAY, (dt_pret+3), GETDATE()) FROM Emprunt WHERE id_livre=3 AND dt_retour IS NULL

SELECT DATEDIFF(DAY, (dt_pret+3), dt_retour) FROM Emprunt WHERE id_livre=3

SELECT COUNT(e.id_livre) from Emprunt e INNER JOIN Membre m ON m.id_membre = e.id_membre GROUP BY e.id_membre, m.nom HAVING m.id_membre=2

SELECT * FROM Membres WHERE id_membre IN (SELECT id_membre FROM Emprunt GROUP BY id_membre HAVING COUNT(id_membre) < 3) OR id_membre NOT IN (SELECT id_membre FROM Emprunt)

SELECT * FROM Livres WHERE id_livre NOT IN(SELECT id_livre FROM Emprunt WHERE dt_retour IS NULL)

SELECT COUNT(e.id_livre) from Emprunt e INNER JOIN Membres m ON m.id_membre = e.id_membre

SELECT TOP 1 DATEDIFF(DAY, (dt_pret + 3), GETDATE()) FROM Emprunt WHERE id_livre = 1 AND id_membre = 8 ORDER BY dt_pret DESC

SELECT TOP 1 DATEDIFF(DAY, (dt_pret + 3), GETDATE()) FROM Emprunt WHERE id_membre = 8 ORDER BY dt_pret DESC

SELECT TOP 1 dt_pret + 3 FROM Emprunt WHERE id_livre = 1 AND id_membre = 8 ORDER BY dt_pret DESC

