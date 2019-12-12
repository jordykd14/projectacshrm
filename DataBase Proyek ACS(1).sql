drop table jabatan cascade CONSTRAINT purge;
drop table pegawai cascade CONSTRAINT purge;
drop table jenis cascade CONSTRAINT purge;
drop table Hnota cascade constraint purge;
drop table dnota cascade constraint Purge;

create table jabatan(
	id_jabatan varchar2(5) primary key,
	nama_jabatan varchar2(10) not null
);

Create table pegawai(
	id_jabatan varchar2(6) references jabatan(id_jabatan) not null,
	id_pegawai varchar2(6) primary key,
	nama_pegawai varchar2(50) not null,
	username varchar2(50) not null,
	pass varchar2(50) not null
);

CREATE TABLE JENIS(
    ID_JENIS varchar2(10) PRIMARY KEY,
    NAMA_JENIS VARCHAR2(30) NOT NULL
); 

create table Hnota(
	id_nota varchar2(15) primary key,
	tanggal_DISERAHKAN date not null,
	TANGGAL_DICLAIM DATE,
	TOTAL NUMBER NOT NULL,
	id_pegawai varchar2(50) references pegawai(id_pegawai) not null,
	ID_MANAGER VARCHAR2(50) REFERENCES PEGAWAI(ID_PEGAWAI),
	STATUS_NOTA VARCHAR2(1) CONSTRAINT CHK_STATUS CHECK(STATUS_NOTA = '1' OR STATUS_NOTA = '0') NOT NULL
);

CREATE table DNOTA(
    ID_NOTA varchar2(15) references HNOTA(ID_NOTA) NOT NULL,
    ID_JENIS VARCHAR2(10) references JENIS(ID_JENIS) NOT NULL,
	tgl_transaksi date,
    HARGA NUMBER NOT NULL,
	PAJAK NUMBER NOT NULL,
	HARGA_BERSIH NUMBER NOT NULL,
    GAMBAR VARCHAR2(100)
);

INSERT INTO JABATAN VALUES('JA001','Manager');
INSERT INTO JABATAN VALUES('JA002','Operator');
INSERT INTO JABATAN VALUES('JA003','SuperVisor');
INSERT INTO JABATAN VALUES('JA004','Sales');
INSERT INTO JABATAN VALUES('JA005','Owner');

INSERT INTO PEGAWAI VALUES('JA001','PDS001','David Suwigyo','david','david');
INSERT INTO PEGAWAI VALUES('JA002','PGJ001','Gracielo Justine','cielo','celo');
INSERT INTO PEGAWAI VALUES('JA002','PSG001','Steven Go','go','go');
INSERT INTO PEGAWAI VALUES('JA003','PMT001','Melvern Tamara','melvern','melvern');
INSERT INTO PEGAWAI VALUES('JA003','PMS001','Maria Seraphinz','maria','maria');
INSERT INTO PEGAWAI VALUES('JA004','PLR001','Lukito Raharjo','lukito','lukito');
INSERT INTO PEGAWAI VALUES('JA004','PKH001','Kevin Hongary','kevin','kevin');
INSERT INTO PEGAWAI VALUES('JA004','PWN001','Wandi Nagata','wandi','wandi');
INSERT INTO PEGAWAI VALUES('JA004','PNJ001','Nathania Josephine','nia','nia');
INSERT INTO PEGAWAI VALUES('JA005','PWR001','Wira Rafi','wira','wira');


INSERT INTO JENIS VALUES('JE001','Transportasi');
INSERT INTO JENIS VALUES('JE002','Makan');
INSERT INTO JENIS VALUES('JE003','Penginapan');
INSERT INTO JENIS VALUES('JE004','Lainnya');

create or replace function fautogenid(id in varchar2)
return varchar2
is
ctr number;
begin
    select count(id_nota) into ctr
    from Hnota
    where id_nota like '%'||id||'%';
    return 'N'||id||'00'||(ctr+1);
end;
/
show err;

create or replace function Autoid(id varchar2)
return varchar2
is 
err exception;
nama VARCHAR2(20);
CTR number;
BEGIN
NAMA:=id;
if length(nama) < 2 then 
raise err;
--untuk auto generet nama
end if;
if instr(upper(nama),' ')>0 then
SELECT COUNT(ID_Pegawai) into CTR
FROM pegawai
WHERE ID_pegawai LIKE 'P'||substr(upper(nama),1,1)||substr(upper(nama),(instr((nama),' '))+1,1)||'%';
CTR:= CTR+1;
    if CTR<10 THEN 
    return('P'||substr(upper(nama),1,1)||substr(upper(nama),(instr((nama),' '))+1,1)||'00'||CTR);
    ELSIF CTR>10 THEN
    return('P'||substr(upper(nama),1,1)||substr(upper(nama),(instr((nama),' '))+1,1)||'0'||CTR);
    END IF;
ELSIF instr(upper(nama),' ')=0 then
    SELECT COUNT(ID_pegawai) into CTR
    FROM pegawai
    WHERE ID_pegawai LIKE 'P'||substr(upper(nama),1,2)||'%';
    CTR:= CTR+1;
    if CTR<10 THEN 
       return('P'||substr(upper(nama),1,2)||'00'||CTR);
    ELSIF CTR>10 THEN
        return('P'||substr(upper(nama),1,2)||'0'||CTR);
end if;
end if;
exception 
    when err then
    raise_application_error(-20001,'Hanya Mengandung 1 huruf');
END;
/

commit;