using AutoDocService.Helpers.Utils;

namespace AutoDocService.DL.FolderParamZaObrisati
{

    public class Placeholders
    {
        public ClientInfo ClientInfo { get; set; }
        public AccountInfo AccountInfo { get; set; }
        public LoanInfo LoanInfo { get; set; }
        public CardInfo CardInfo { get; set; }
        public ContactInfo ContactInfo { get; set; }
        public EmploymentInfo EmploymentInfo { get; set; }
        public AddressInfo AddressInfo { get; set; }
        public GuarantorInfo GuarantorInfo { get; set; }
        public CollateralInfo CollateralInfo { get; set; }
        public PaymentScheduleInfo PaymentScheduleInfo { get; set; }
        public InterestInfo InterestInfo { get; set; }
        public FeeInfo FeeInfo { get; set; }
        public BankBranchInfo BankBranchInfo { get; set; }
        public ContractInfo ContractInfo { get; set; }
        public LegalInfo LegalInfo { get; set; }
        public NotificationInfo NotificationInfo { get; set; }
        public StatementInfo StatementInfo { get; set; }
        public TransactionInfo TransactionInfo { get; set; }
        public LimitInfo LimitInfo { get; set; }
        public CurrencyInfo CurrencyInfo { get; set; }
        public ExchangeRateInfo ExchangeRateInfo { get; set; }
        public InsuranceInfo InsuranceInfo { get; set; }
        public TaxInfo TaxInfo { get; set; }
        public PenaltyInfo PenaltyInfo { get; set; }
        public RepaymentInfo RepaymentInfo { get; set; }
        public OverdraftInfo OverdraftInfo { get; set; }
        public DepositInfo DepositInfo { get; set; }
        public SavingsInfo SavingsInfo { get; set; }
        public InvestmentInfo InvestmentInfo { get; set; }
        public PowerOfAttorneyInfo PowerOfAttorneyInfo { get; set; }
        public DocumentInfo DocumentInfo { get; set; }
        public SignatureInfo SignatureInfo { get; set; }
        public ApprovalInfo ApprovalInfo { get; set; }
        public DisbursementInfo DisbursementInfo { get; set; }
        public ClosureInfo ClosureInfo { get; set; }
        public AmendmentInfo AmendmentInfo { get; set; }
        public ConsentInfo ConsentInfo { get; set; }
        public RiskAssessmentInfo RiskAssessmentInfo { get; set; }
        public ComplianceInfo ComplianceInfo { get; set; }
        public AuditInfo AuditInfo { get; set; }
        public NotificationSettings NotificationSettings { get; set; }
    }

    public class ClientInfo
    {
        [Placeholder("Ime klijenta", "Puno ime klijenta", "string")]
        public string FirstName { get; set; }
        [Placeholder("Prezime klijenta", "Puno prezime klijenta", "string")]
        public string LastName { get; set; }
        [Placeholder("JMBG", "Jedinstveni matični broj građana", "string")]
        public string PersonalId { get; set; }
        [Placeholder("Datum rođenja", "Datum rođenja klijenta", "DateTime")]
        public DateTime DateOfBirth { get; set; }
        [Placeholder("Email adresa", "Kontakt email klijenta", "string")]
        public string Email { get; set; }
    }

    public class AccountInfo
    {
        [Placeholder("Broj računa", "Jedinstveni broj bankovnog računa", "string")]
        public string AccountNumber { get; set; }
        [Placeholder("Tip računa", "Vrsta bankovnog računa", "string")]
        public string AccountType { get; set; }
        [Placeholder("Stanje računa", "Trenutno stanje na računu", "decimal")]
        public decimal Balance { get; set; }
        [Placeholder("Datum otvaranja", "Datum kada je račun otvoren", "DateTime")]
        public DateTime OpenDate { get; set; }
        [Placeholder("Status računa", "Status bankovnog računa", "string")]
        public string Status { get; set; }
    }

    public class LoanInfo
    {
        [Placeholder("Iznos kredita", "Ukupan iznos odobrenog kredita", "decimal")]
        public decimal LoanAmount { get; set; }
        [Placeholder("Rok otplate", "Datum završetka otplate kredita", "DateTime")]
        public DateTime MaturityDate { get; set; }
        [Placeholder("Kamatna stopa", "Godišnja kamatna stopa", "decimal")]
        public decimal InterestRate { get; set; }
        [Placeholder("Broj rata", "Ukupan broj rata za otplatu", "int")]
        public int NumberOfInstallments { get; set; }
        [Placeholder("Status kredita", "Trenutni status kredita", "string")]
        public string Status { get; set; }
    }

    public class CardInfo
    {
        [Placeholder("Broj kartice", "Jedinstveni broj platne kartice", "string")]
        public string CardNumber { get; set; }
        [Placeholder("Tip kartice", "Vrsta platne kartice (Visa, MasterCard...)", "string")]
        public string CardType { get; set; }
        [Placeholder("Datum isteka", "Datum isteka kartice", "DateTime")]
        public DateTime ExpiryDate { get; set; }
        [Placeholder("Status kartice", "Status platne kartice", "string")]
        public string Status { get; set; }
        [Placeholder("Limit kartice", "Maksimalni dozvoljeni limit", "decimal")]
        public decimal CardLimit { get; set; }
    }

    public class ContactInfo
    {
        [Placeholder("Telefon", "Broj telefona klijenta", "string")]
        public string Phone { get; set; }
        [Placeholder("Mobilni telefon", "Broj mobilnog telefona klijenta", "string")]
        public string Mobile { get; set; }
        [Placeholder("Adresa", "Adresa stanovanja klijenta", "string")]
        public string Address { get; set; }
        [Placeholder("Grad", "Grad prebivališta", "string")]
        public string City { get; set; }
        [Placeholder("Poštanski broj", "Poštanski broj prebivališta", "string")]
        public string PostalCode { get; set; }
    }

    public class EmploymentInfo
    {
        [Placeholder("Naziv poslodavca", "Naziv firme u kojoj je klijent zaposlen", "string")]
        public string EmployerName { get; set; }
        [Placeholder("Pozicija", "Radno mjesto klijenta", "string")]
        public string Position { get; set; }
        [Placeholder("Mjesečna primanja", "Iznos mjesečnih primanja", "decimal")]
        public decimal MonthlyIncome { get; set; }
        [Placeholder("Status zaposlenja", "Trenutni status zaposlenja", "string")]
        public string EmploymentStatus { get; set; }
        [Placeholder("Godina zaposlenja", "Godina kada je klijent zaposlen", "int")]
        public int EmploymentYear { get; set; }
    }

    public class AddressInfo
    {
        [Placeholder("Ulica", "Naziv ulice prebivališta", "string")]
        public string Street { get; set; }
        [Placeholder("Broj", "Kućni broj", "string")]
        public string Number { get; set; }
        [Placeholder("Grad", "Grad prebivališta", "string")]
        public string City { get; set; }
        [Placeholder("Poštanski broj", "Poštanski broj", "string")]
        public string PostalCode { get; set; }
        [Placeholder("Država", "Država prebivališta", "string")]
        public string Country { get; set; }
    }

    public class GuarantorInfo
    {
        [Placeholder("Ime jamca", "Puno ime jamca", "string")]
        public string GuarantorName { get; set; }
        [Placeholder("JMBG jamca", "Jedinstveni matični broj jamca", "string")]
        public string GuarantorId { get; set; }
        [Placeholder("Telefon jamca", "Kontakt telefon jamca", "string")]
        public string GuarantorPhone { get; set; }
        [Placeholder("Adresa jamca", "Adresa stanovanja jamca", "string")]
        public string GuarantorAddress { get; set; }
        [Placeholder("Status jamca", "Status jamca u ugovoru", "string")]
        public string GuarantorStatus { get; set; }
    }

    public class CollateralInfo
    {
        [Placeholder("Tip kolaterala", "Vrsta ponuđenog kolaterala", "string")]
        public string CollateralType { get; set; }
        [Placeholder("Vrijednost kolaterala", "Procijenjena vrijednost kolaterala", "decimal")]
        public decimal CollateralValue { get; set; }
        [Placeholder("Opis kolaterala", "Detaljan opis kolaterala", "string")]
        public string CollateralDescription { get; set; }
        [Placeholder("Status kolaterala", "Status procjene kolaterala", "string")]
        public string CollateralStatus { get; set; }
        [Placeholder("Datum procjene", "Datum procjene kolaterala", "DateTime")]
        public DateTime AppraisalDate { get; set; }
    }

    public class PaymentScheduleInfo
    {
        [Placeholder("Broj rata", "Ukupan broj rata", "int")]
        public int NumberOfInstallments { get; set; }
        [Placeholder("Iznos rate", "Iznos pojedinačne rate", "decimal")]
        public decimal InstallmentAmount { get; set; }
        [Placeholder("Datum prve rate", "Datum dospijeća prve rate", "DateTime")]
        public DateTime FirstInstallmentDate { get; set; }
        [Placeholder("Datum zadnje rate", "Datum dospijeća zadnje rate", "DateTime")]
        public DateTime LastInstallmentDate { get; set; }
        [Placeholder("Status otplate", "Status otplate kredita", "string")]
        public string RepaymentStatus { get; set; }
    }

    public class InterestInfo
    {
        [Placeholder("Kamatna stopa", "Godišnja kamatna stopa", "decimal")]
        public decimal InterestRate { get; set; }
        [Placeholder("Vrsta kamate", "Fiksna ili promjenjiva kamata", "string")]
        public string InterestType { get; set; }
        [Placeholder("Datum početka", "Datum početka obračuna kamate", "DateTime")]
        public DateTime StartDate { get; set; }
        [Placeholder("Datum završetka", "Datum završetka obračuna kamate", "DateTime")]
        public DateTime EndDate { get; set; }
        [Placeholder("Status kamate", "Status obračuna kamate", "string")]
        public string InterestStatus { get; set; }
    }

    public class FeeInfo
    {
        [Placeholder("Vrsta naknade", "Tip bankarske naknade", "string")]
        public string FeeType { get; set; }
        [Placeholder("Iznos naknade", "Iznos naknade", "decimal")]
        public decimal FeeAmount { get; set; }
        [Placeholder("Datum naplate", "Datum naplate naknade", "DateTime")]
        public DateTime FeeDate { get; set; }
        [Placeholder("Status naknade", "Status naplate naknade", "string")]
        public string FeeStatus { get; set; }
        [Placeholder("Opis naknade", "Dodatni opis naknade", "string")]
        public string FeeDescription { get; set; }
    }

    public class BankBranchInfo
    {
        [Placeholder("Naziv filijale", "Naziv bankarske filijale", "string")]
        public string BranchName { get; set; }
        [Placeholder("Adresa filijale", "Adresa bankarske filijale", "string")]
        public string BranchAddress { get; set; }
        [Placeholder("Grad filijale", "Grad u kojem se nalazi filijala", "string")]
        public string BranchCity { get; set; }
        [Placeholder("Telefon filijale", "Kontakt telefon filijale", "string")]
        public string BranchPhone { get; set; }
        [Placeholder("Šifra filijale", "Jedinstvena šifra filijale", "string")]
        public string BranchCode { get; set; }
    }

    public class ContractInfo
    {
        [Placeholder("Broj ugovora", "Jedinstveni broj ugovora", "string")]
        public string ContractNumber { get; set; }
        [Placeholder("Datum ugovora", "Datum potpisivanja ugovora", "DateTime")]
        public DateTime ContractDate { get; set; }
        [Placeholder("Status ugovora", "Trenutni status ugovora", "string")]
        public string ContractStatus { get; set; }
        [Placeholder("Tip ugovora", "Vrsta bankarskog ugovora", "string")]
        public string ContractType { get; set; }
        [Placeholder("Opis ugovora", "Dodatni opis ugovora", "string")]
        public string ContractDescription { get; set; }
    }

    public class LegalInfo
    {
        [Placeholder("Naziv pravnog lica", "Naziv firme ili pravnog subjekta", "string")]
        public string LegalEntityName { get; set; }
        [Placeholder("ID pravnog lica", "Jedinstveni identifikacioni broj", "string")]
        public string LegalEntityId { get; set; }
        [Placeholder("Adresa pravnog lica", "Adresa pravnog subjekta", "string")]
        public string LegalEntityAddress { get; set; }
        [Placeholder("Telefon pravnog lica", "Kontakt telefon pravnog subjekta", "string")]
        public string LegalEntityPhone { get; set; }
        [Placeholder("Status pravnog lica", "Status pravnog subjekta", "string")]
        public string LegalEntityStatus { get; set; }
    }

    public class NotificationInfo
    {
        [Placeholder("Tip obavijesti", "Vrsta obavijesti (email, SMS...)", "string")]
        public string NotificationType { get; set; }
        [Placeholder("Sadržaj obavijesti", "Tekst obavijesti", "string")]
        public string NotificationContent { get; set; }
        [Placeholder("Datum slanja", "Datum slanja obavijesti", "DateTime")]
        public DateTime NotificationDate { get; set; }
        [Placeholder("Status obavijesti", "Status isporuke obavijesti", "string")]
        public string NotificationStatus { get; set; }
        [Placeholder("Primalac", "Ime i prezime primaoca", "string")]
        public string Recipient { get; set; }
    }

    public class StatementInfo
    {
        [Placeholder("Broj izvoda", "Jedinstveni broj bankarskog izvoda", "string")]
        public string StatementNumber { get; set; }
        [Placeholder("Datum izvoda", "Datum izdavanja izvoda", "DateTime")]
        public DateTime StatementDate { get; set; }
        [Placeholder("Stanje na izvještaju", "Stanje računa na dan izvoda", "decimal")]
        public decimal StatementBalance { get; set; }
        [Placeholder("Tip izvoda", "Vrsta bankarskog izvoda", "string")]
        public string StatementType { get; set; }
        [Placeholder("Status izvoda", "Status izvoda", "string")]
        public string StatementStatus { get; set; }
    }

    public class TransactionInfo
    {
        [Placeholder("Broj transakcije", "Jedinstveni broj transakcije", "string")]
        public string TransactionNumber { get; set; }
        [Placeholder("Datum transakcije", "Datum izvršenja transakcije", "DateTime")]
        public DateTime TransactionDate { get; set; }
        [Placeholder("Iznos transakcije", "Iznos transakcije", "decimal")]
        public decimal TransactionAmount { get; set; }
        [Placeholder("Tip transakcije", "Vrsta transakcije (uplata, isplata...)", "string")]
        public string TransactionType { get; set; }
        [Placeholder("Status transakcije", "Status transakcije", "string")]
        public string TransactionStatus { get; set; }
    }

    public class LimitInfo
    {
        [Placeholder("Tip limita", "Vrsta limita (dnevni, mjesečni...)", "string")]
        public string LimitType { get; set; }
        [Placeholder("Iznos limita", "Maksimalni dozvoljeni iznos", "decimal")]
        public decimal LimitAmount { get; set; }
        [Placeholder("Datum postavljanja", "Datum postavljanja limita", "DateTime")]
        public DateTime LimitDate { get; set; }
        [Placeholder("Status limita", "Status limita", "string")]
        public string LimitStatus { get; set; }
        [Placeholder("Opis limita", "Dodatni opis limita", "string")]
        public string LimitDescription { get; set; }
    }

    public class CurrencyInfo
    {
        [Placeholder("Šifra valute", "ISO šifra valute (npr. BAM, EUR)", "string")]
        public string CurrencyCode { get; set; }
        [Placeholder("Naziv valute", "Naziv valute", "string")]
        public string CurrencyName { get; set; }
        [Placeholder("Simbol valute", "Simbol valute (npr. KM, €)", "string")]
        public string CurrencySymbol { get; set; }
        [Placeholder("Status valute", "Status valute", "string")]
        public string CurrencyStatus { get; set; }
        [Placeholder("Kurs valute", "Trenutni kurs valute", "decimal")]
        public decimal ExchangeRate { get; set; }
    }

    public class ExchangeRateInfo
    {
        [Placeholder("Valuta", "Valuta za koju se prikazuje kurs", "string")]
        public string Currency { get; set; }
        [Placeholder("Referentna valuta", "Valuta prema kojoj se računa kurs", "string")]
        public string ReferenceCurrency { get; set; }
        [Placeholder("Vrijednost kursa", "Vrijednost kursa", "decimal")]
        public decimal RateValue { get; set; }
        [Placeholder("Datum kursa", "Datum važenja kursa", "DateTime")]
        public DateTime RateDate { get; set; }
        [Placeholder("Status kursa", "Status kursa", "string")]
        public string RateStatus { get; set; }
    }

    public class InsuranceInfo
    {
        [Placeholder("Naziv osiguranja", "Naziv police osiguranja", "string")]
        public string InsuranceName { get; set; }
        [Placeholder("Broj police", "Jedinstveni broj police osiguranja", "string")]
        public string PolicyNumber { get; set; }
        [Placeholder("Iznos osiguranja", "Ukupan iznos osiguranja", "decimal")]
        public decimal InsuranceAmount { get; set; }
        [Placeholder("Datum početka", "Datum početka osiguranja", "DateTime")]
        public DateTime StartDate { get; set; }
        [Placeholder("Status osiguranja", "Status police osiguranja", "string")]
        public string InsuranceStatus { get; set; }
    }

    public class TaxInfo
    {
        [Placeholder("Tip poreza", "Vrsta poreza", "string")]
        public string TaxType { get; set; }
        [Placeholder("Iznos poreza", "Ukupan iznos poreza", "decimal")]
        public decimal TaxAmount { get; set; }
        [Placeholder("Datum obračuna", "Datum obračuna poreza", "DateTime")]
        public DateTime TaxDate { get; set; }
        [Placeholder("Status poreza", "Status obračuna poreza", "string")]
        public string TaxStatus { get; set; }
        [Placeholder("Opis poreza", "Dodatni opis poreza", "string")]
        public string TaxDescription { get; set; }
    }

    public class PenaltyInfo
    {
        [Placeholder("Tip penala", "Vrsta penala", "string")]
        public string PenaltyType { get; set; }
        [Placeholder("Iznos penala", "Ukupan iznos penala", "decimal")]
        public decimal PenaltyAmount { get; set; }
        [Placeholder("Datum penala", "Datum obračuna penala", "DateTime")]
        public DateTime PenaltyDate { get; set; }
        [Placeholder("Status penala", "Status obračuna penala", "string")]
        public string PenaltyStatus { get; set; }
        [Placeholder("Opis penala", "Dodatni opis penala", "string")]
        public string PenaltyDescription { get; set; }
    }

    public class RepaymentInfo
    {
        [Placeholder("Iznos otplate", "Iznos pojedinačne otplate", "decimal")]
        public decimal RepaymentAmount { get; set; }
        [Placeholder("Datum otplate", "Datum izvršenja otplate", "DateTime")]
        public DateTime RepaymentDate { get; set; }
        [Placeholder("Status otplate", "Status otplate", "string")]
        public string RepaymentStatus { get; set; }
        [Placeholder("Tip otplate", "Vrsta otplate", "string")]
        public string RepaymentType { get; set; }
        [Placeholder("Opis otplate", "Dodatni opis otplate", "string")]
        public string RepaymentDescription { get; set; }
    }

    public class OverdraftInfo
    {
        [Placeholder("Iznos dozvoljenog minusa", "Maksimalni iznos dozvoljenog minusa", "decimal")]
        public decimal OverdraftAmount { get; set; }
        [Placeholder("Datum odobrenja", "Datum odobrenja minusa", "DateTime")]
        public DateTime ApprovalDate { get; set; }
        [Placeholder("Status minusa", "Status dozvoljenog minusa", "string")]
        public string OverdraftStatus { get; set; }
        [Placeholder("Tip minusa", "Vrsta dozvoljenog minusa", "string")]
        public string OverdraftType { get; set; }
        [Placeholder("Opis minusa", "Dodatni opis minusa", "string")]
        public string OverdraftDescription { get; set; }
    }

    public class DepositInfo
    {
        [Placeholder("Iznos depozita", "Ukupan iznos depozita", "decimal")]
        public decimal DepositAmount { get; set; }
        [Placeholder("Datum depozita", "Datum uplate depozita", "DateTime")]
        public DateTime DepositDate { get; set; }
        [Placeholder("Status depozita", "Status depozita", "string")]
        public string DepositStatus { get; set; }
        [Placeholder("Tip depozita", "Vrsta depozita", "string")]
        public string DepositType { get; set; }
        [Placeholder("Opis depozita", "Dodatni opis depozita", "string")]
        public string DepositDescription { get; set; }
    }

    public class SavingsInfo
    {
        [Placeholder("Iznos štednje", "Ukupan iznos štednje", "decimal")]
        public decimal SavingsAmount { get; set; }
        [Placeholder("Datum štednje", "Datum uplate štednje", "DateTime")]
        public DateTime SavingsDate { get; set; }
        [Placeholder("Status štednje", "Status štednje", "string")]
        public string SavingsStatus { get; set; }
        [Placeholder("Tip štednje", "Vrsta štednje", "string")]
        public string SavingsType { get; set; }
        [Placeholder("Opis štednje", "Dodatni opis štednje", "string")]
        public string SavingsDescription { get; set; }
    }

    public class InvestmentInfo
    {
        [Placeholder("Iznos investicije", "Ukupan iznos investicije", "decimal")]
        public decimal InvestmentAmount { get; set; }
        [Placeholder("Datum investicije", "Datum ulaganja", "DateTime")]
        public DateTime InvestmentDate { get; set; }
        [Placeholder("Status investicije", "Status investicije", "string")]
        public string InvestmentStatus { get; set; }
        [Placeholder("Tip investicije", "Vrsta investicije", "string")]
        public string InvestmentType { get; set; }
        [Placeholder("Opis investicije", "Dodatni opis investicije", "string")]
        public string InvestmentDescription { get; set; }
    }

    public class PowerOfAttorneyInfo
    {
        [Placeholder("Ime punomoćnika", "Puno ime punomoćnika", "string")]
        public string AttorneyName { get; set; }
        [Placeholder("JMBG punomoćnika", "Jedinstveni matični broj punomoćnika", "string")]
        public string AttorneyId { get; set; }
        [Placeholder("Tip punomoći", "Vrsta punomoći", "string")]
        public string AttorneyType { get; set; }
        [Placeholder("Datum izdavanja", "Datum izdavanja punomoći", "DateTime")]
        public DateTime IssueDate { get; set; }
        [Placeholder("Status punomoći", "Status punomoći", "string")]
        public string AttorneyStatus { get; set; }
    }

    public class DocumentInfo
    {
        [Placeholder("Naziv dokumenta", "Naziv bankarskog dokumenta", "string")]
        public string DocumentName { get; set; }
        [Placeholder("Broj dokumenta", "Jedinstveni broj dokumenta", "string")]
        public string DocumentNumber { get; set; }
        [Placeholder("Datum izdavanja", "Datum izdavanja dokumenta", "DateTime")]
        public DateTime IssueDate { get; set; }
        [Placeholder("Status dokumenta", "Status dokumenta", "string")]
        public string DocumentStatus { get; set; }
        [Placeholder("Tip dokumenta", "Vrsta dokumenta", "string")]
        public string DocumentType { get; set; }
    }

    public class SignatureInfo
    {
        [Placeholder("Ime potpisnika", "Puno ime potpisnika", "string")]
        public string SignerName { get; set; }
        [Placeholder("Datum potpisa", "Datum potpisivanja dokumenta", "DateTime")]
        public DateTime SignatureDate { get; set; }
        [Placeholder("Status potpisa", "Status potpisa", "string")]
        public string SignatureStatus { get; set; }
        [Placeholder("Tip potpisa", "Vrsta potpisa", "string")]
        public string SignatureType { get; set; }
        [Placeholder("Opis potpisa", "Dodatni opis potpisa", "string")]
        public string SignatureDescription { get; set; }
    }

    public class ApprovalInfo
    {
        [Placeholder("Ime odobravaoca", "Puno ime osobe koja odobrava", "string")]
        public string ApproverName { get; set; }
        [Placeholder("Datum odobrenja", "Datum odobravanja", "DateTime")]
        public DateTime ApprovalDate { get; set; }
        [Placeholder("Status odobrenja", "Status odobrenja", "string")]
        public string ApprovalStatus { get; set; }
        [Placeholder("Tip odobrenja", "Vrsta odobrenja", "string")]
        public string ApprovalType { get; set; }
        [Placeholder("Opis odobrenja", "Dodatni opis odobrenja", "string")]
        public string ApprovalDescription { get; set; }
    }

    public class DisbursementInfo
    {
        [Placeholder("Iznos isplate", "Ukupan iznos isplate", "decimal")]
        public decimal DisbursementAmount { get; set; }
        [Placeholder("Datum isplate", "Datum izvršenja isplate", "DateTime")]
        public DateTime DisbursementDate { get; set; }
        [Placeholder("Status isplate", "Status isplate", "string")]
        public string DisbursementStatus { get; set; }
        [Placeholder("Tip isplate", "Vrsta isplate", "string")]
        public string DisbursementType { get; set; }
        [Placeholder("Opis isplate", "Dodatni opis isplate", "string")]
        public string DisbursementDescription { get; set; }
    }

    public class ClosureInfo
    {
        [Placeholder("Datum zatvaranja", "Datum zatvaranja računa/ugovora", "DateTime")]
        public DateTime ClosureDate { get; set; }
        [Placeholder("Status zatvaranja", "Status zatvaranja", "string")]
        public string ClosureStatus { get; set; }
        [Placeholder("Tip zatvaranja", "Vrsta zatvaranja", "string")]
        public string ClosureType { get; set; }
        [Placeholder("Opis zatvaranja", "Dodatni opis zatvaranja", "string")]
        public string ClosureDescription { get; set; }
        [Placeholder("Razlog zatvaranja", "Razlog zatvaranja", "string")]
        public string ClosureReason { get; set; }
    }

    public class AmendmentInfo
    {
        [Placeholder("Broj aneksa", "Jedinstveni broj aneksa", "string")]
        public string AmendmentNumber { get; set; }
        [Placeholder("Datum aneksa", "Datum potpisivanja aneksa", "DateTime")]
        public DateTime AmendmentDate { get; set; }
        [Placeholder("Status aneksa", "Status aneksa", "string")]
        public string AmendmentStatus { get; set; }
        [Placeholder("Tip aneksa", "Vrsta aneksa", "string")]
        public string AmendmentType { get; set; }
        [Placeholder("Opis aneksa", "Dodatni opis aneksa", "string")]
        public string AmendmentDescription { get; set; }
    }

    public class ConsentInfo
    {
        [Placeholder("Tip saglasnosti", "Vrsta saglasnosti", "string")]
        public string ConsentType { get; set; }
        [Placeholder("Datum saglasnosti", "Datum davanja saglasnosti", "DateTime")]
        public DateTime ConsentDate { get; set; }
        [Placeholder("Status saglasnosti", "Status saglasnosti", "string")]
        public string ConsentStatus { get; set; }
        [Placeholder("Opis saglasnosti", "Dodatni opis saglasnosti", "string")]
        public string ConsentDescription { get; set; }
        [Placeholder("Ime davaoca", "Puno ime davaoca saglasnosti", "string")]
        public string ConsentGiver { get; set; }
    }

    public class RiskAssessmentInfo
    {
        [Placeholder("Tip procjene rizika", "Vrsta procjene rizika", "string")]
        public string RiskType { get; set; }
        [Placeholder("Datum procjene", "Datum procjene rizika", "DateTime")]
        public DateTime RiskDate { get; set; }
        [Placeholder("Status procjene", "Status procjene rizika", "string")]
        public string RiskStatus { get; set; }
        [Placeholder("Opis rizika", "Dodatni opis rizika", "string")]
        public string RiskDescription { get; set; }
        [Placeholder("Procjenitelj", "Ime osobe koja je izvršila procjenu", "string")]
        public string Assessor { get; set; }
    }

    public class ComplianceInfo
    {
        [Placeholder("Tip usklađenosti", "Vrsta regulatorne usklađenosti", "string")]
        public string ComplianceType { get; set; }
        [Placeholder("Datum usklađenosti", "Datum provjere usklađenosti", "DateTime")]
        public DateTime ComplianceDate { get; set; }
        [Placeholder("Status usklađenosti", "Status usklađenosti", "string")]
        public string ComplianceStatus { get; set; }
        [Placeholder("Opis usklađenosti", "Dodatni opis usklađenosti", "string")]
        public string ComplianceDescription { get; set; }
        [Placeholder("Ime provjeravaoca", "Puno ime osobe koja je izvršila provjeru", "string")]
        public string ComplianceOfficer { get; set; }
    }

    public class AuditInfo
    {
        [Placeholder("Tip revizije", "Vrsta revizije", "string")]
        public string AuditType { get; set; }
        [Placeholder("Datum revizije", "Datum izvršenja revizije", "DateTime")]
        public DateTime AuditDate { get; set; }
        [Placeholder("Status revizije", "Status revizije", "string")]
        public string AuditStatus { get; set; }
        [Placeholder("Opis revizije", "Dodatni opis revizije", "string")]
        public string AuditDescription { get; set; }
        [Placeholder("Ime revizora", "Puno ime revizora", "string")]
        public string Auditor { get; set; }
    }

    public class NotificationSettings
    {
        [Placeholder("Email obavijesti", "Status email obavijesti", "bool")]
        public bool EmailEnabled { get; set; }
        [Placeholder("SMS obavijesti", "Status SMS obavijesti", "bool")]
        public bool SmsEnabled { get; set; }
        [Placeholder("Push obavijesti", "Status push obavijesti", "bool")]
        public bool PushEnabled { get; set; }
        [Placeholder("Jezik obavijesti", "Jezik na kojem se šalju obavijesti", "string")]
        public string NotificationLanguage { get; set; }
        [Placeholder("Vrijeme slanja", "Preferirano vrijeme slanja obavijesti", "string")]
        public string NotificationTime { get; set; }
    }
}