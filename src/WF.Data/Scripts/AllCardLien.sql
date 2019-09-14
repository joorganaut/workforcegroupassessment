SELECT
    g.foracid        AccountNumber,
	g.ACCT_NAME      AccountName,
    l.b2k_id         Reference,
    g.acid           LienAccount,
    l.lien_amt       Amount,
    l.lien_remarks   Narration
FROM
    tbaadm.alt l,
    tbaadm.gam g
WHERE
    l.acid = g.acid
	AND l.b2k_id = 'CardChrgLien'
	AND g.foracid = NVL('{accountNumber}', g.foracid)