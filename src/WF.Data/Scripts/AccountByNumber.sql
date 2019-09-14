SELECT
    foracid as AccountNumber,
    a.acct_name as Name,
    a.cif_id as CustomerID,
    a.schm_code as ProductCode,
    clr_bal_amt,
    ( least(a.sanct_lim, a.drwng_power) + ( clr_bal_amt - lien_amt - system_reserved_amt + single_tran_lim + clean_adhoc_lim + clean_emer_advn
    + clean_single_tran_lim) ) as Balance,
    lien_amt as Lien,
    future_bal_amt,
    a.sanct_lim,
    a.drwng_power,
    a.acct_crncy_code as Currency,
    b.schm_desc as ProductName,
    CASE
        WHEN a.schm_type = 'SBA' THEN (
            SELECT
                acct_status
            FROM
                tbaadm.smt d,
                tbaadm.gam e
            WHERE
                d.acid = e.acid
                AND e.foracid = a.foracid
        )
        ELSE (
            SELECT
                acct_status
            FROM
                tbaadm.cam d,
                tbaadm.gam e
            WHERE
                d.acid = e.acid
                AND e.foracid = a.foracid
        )
    END accountstatus
FROM
    tbaadm.gam a,
    tbaadm.gsp b 
WHERE
    a.schm_code = b.schm_code
                and foracid = '{accountNumber}'