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
        [Placeholder("Ime klijenta", "Puno ime klijenta")]
        public string FirstName { get; set; }
        [Placeholder("Prezime klijenta", "Puno prezime klijenta")]
        public string LastName { get; set; }
        [Placeholder("JMBG", "Jedinstveni matični broj građana")]
        public string PersonalId { get; set; }
        [Placeholder("Datum rođenja", "Datum rođenja klijenta")]
        public DateTime DateOfBirth { get; set; }
        [Placeholder("Email adresa", "Kontakt email klijenta")]
        public string Email { get; set; }
    }

    public class AccountInfo
    {
        [Placeholder("Broj računa", "Jedinstveni broj bankovnog računa")]
        public string AccountNumber { get; set; }
        [Placeholder("Tip računa", "Vrsta bankovnog računa")]
        public string AccountType { get; set; }
        [Placeholder("Stanje računa", "Trenutno stanje na računu")]
        public decimal Balance { get; set; }
        [Placeholder("Datum otvaranja", "Datum kada je račun otvoren")]
        public DateTime OpenDate { get; set; }
        [Placeholder("Status računa", "Status bankovnog računa")]
        public string Status { get; set; }
    }

    public class LoanInfo
    {
        [Placeholder("Iznos kredita", "Ukupan iznos odobrenog kredita")]
        public decimal LoanAmount { get; set; }
        [Placeholder("Rok otplate", "Datum završetka otplate kredita")]
        public DateTime MaturityDate { get; set; }
        [Placeholder("Kamatna stopa", "Godišnja kamatna stopa")]
        public decimal InterestRate { get; set; }
        [Placeholder("Broj rata", "Ukupan broj rata za otplatu")]
        public int NumberOfInstallments { get; set; }
        [Placeholder("Status kredita", "Trenutni status kredita")]
        public string Status { get; set; }
    }

    public class CardInfo
    {
        [Placeholder("Broj kartice", "Jedinstveni broj platne kartice")]
        public string CardNumber { get; set; }
        [Placeholder("Tip kartice", "Vrsta platne kartice (Visa, MasterCard...)")]
        public string CardType { get; set; }
        [Placeholder("Datum isteka", "Datum isteka kartice")]
        public DateTime ExpiryDate { get; set; }
        [Placeholder("Status kartice", "Status platne kartice")]
        public string Status { get; set; }
        [Placeholder("Limit kartice", "Maksimalni dozvoljeni limit")]
        public decimal CardLimit { get; set; }
    }

    public class ContactInfo
    {
        [Placeholder("Telefon", "Broj telefona klijenta")]
        public string Phone { get; set; }
        [Placeholder("Mobilni telefon", "Broj mobilnog telefona klijenta")]
        public string Mobile { get; set; }
        [Placeholder("Adresa", "Adresa stanovanja klijenta")]
        public string Address { get; set; }
        [Placeholder("Grad", "Grad prebivališta")]
        public string City { get; set; }
        [Placeholder("Poštanski broj", "Poštanski broj prebivališta")]
        public string PostalCode { get; set; }
    }

    public class EmploymentInfo
    {
        [Placeholder("Naziv poslodavca", "Naziv firme u kojoj je klijent zaposlen")]
        public string EmployerName { get; set; }
        [Placeholder("Pozicija", "Radno mjesto klijenta")]
        public string Position { get; set; }
        [Placeholder("Mjesečna primanja", "Iznos mjesečnih primanja")]
        public decimal MonthlyIncome { get; set; }
        [Placeholder("Status zaposlenja", "Trenutni status zaposlenja")]
        public EmploymentStatusEnum EmploymentStatus { get; set; }
        [Placeholder("Godina zaposlenja", "Godina kada je klijent zaposlen")]
        public int EmploymentYear { get; set; }
    }

    public class AddressInfo
    {
        [Placeholder("Ulica", "Naziv ulice prebivališta")]
        public string Street { get; set; }
        [Placeholder("Broj", "Kućni broj")]
        public string Number { get; set; }
        [Placeholder("Grad", "Grad prebivališta")]
        public string City { get; set; }
        [Placeholder("Poštanski broj", "Poštanski broj")]
        public string PostalCode { get; set; }
        [Placeholder("Država", "Država prebivališta")]
        public string Country { get; set; }
    }

    public class GuarantorInfo
    {
        [Placeholder("Ime jamca", "Puno ime jamca")]
        public string GuarantorName { get; set; }
        [Placeholder("JMBG jamca", "Jedinstveni matični broj jamca")]
        public string GuarantorId { get; set; }
        [Placeholder("Telefon jamca", "Kontakt telefon jamca")]
        public string GuarantorPhone { get; set; }
        [Placeholder("Adresa jamca", "Adresa stanovanja jamca")]
        public string GuarantorAddress { get; set; }
        [Placeholder("Status jamca", "Status jamca u ugovoru")]
        public string GuarantorStatus { get; set; }
    }

    public class CollateralInfo
    {
        [Placeholder("Tip kolaterala", "Vrsta ponuđenog kolaterala")]
        public string CollateralType { get; set; }
        [Placeholder("Vrijednost kolaterala", "Procijenjena vrijednost kolaterala")]
        public decimal CollateralValue { get; set; }
        [Placeholder("Opis kolaterala", "Detaljan opis kolaterala")]
        public string CollateralDescription { get; set; }
        [Placeholder("Status kolaterala", "Status procjene kolaterala")]
        public string CollateralStatus { get; set; }
        [Placeholder("Datum procjene", "Datum procjene kolaterala")]
        public DateTime AppraisalDate { get; set; }
    }

    public class PaymentScheduleInfo
    {
        [Placeholder("Broj rata", "Ukupan broj rata")]
        public int NumberOfInstallments { get; set; }
        [Placeholder("Iznos rate", "Iznos pojedinačne rate")]
        public decimal InstallmentAmount { get; set; }
        [Placeholder("Datum prve rate", "Datum dospijeća prve rate")]
        public DateTime FirstInstallmentDate { get; set; }
        [Placeholder("Datum zadnje rate", "Datum dospijeća zadnje rate")]
        public DateTime LastInstallmentDate { get; set; }
        [Placeholder("Status otplate", "Status otplate kredita")]
        public string RepaymentStatus { get; set; }
    }

    public class InterestInfo
    {
        [Placeholder("Kamatna stopa", "Godišnja kamatna stopa")]
        public decimal InterestRate { get; set; }
        [Placeholder("Vrsta kamate", "Fiksna ili promjenjiva kamata")]
        public string InterestType { get; set; }
        [Placeholder("Datum početka", "Datum početka obračuna kamate")]
        public DateTime StartDate { get; set; }
        [Placeholder("Datum završetka", "Datum završetka obračuna kamate")]
        public DateTime EndDate { get; set; }
        [Placeholder("Status kamate", "Status obračuna kamate")]
        public string InterestStatus { get; set; }
    }

    public class FeeInfo
    {
        [Placeholder("Vrsta naknade", "Tip bankarske naknade")]
        public string FeeType { get; set; }
        [Placeholder("Iznos naknade", "Iznos naknade")]
        public decimal FeeAmount { get; set; }
        [Placeholder("Datum naplate", "Datum naplate naknade")]
        public DateTime FeeDate { get; set; }
        [Placeholder("Status naknade", "Status naplate naknade")]
        public string FeeStatus { get; set; }
        [Placeholder("Opis naknade", "Dodatni opis naknade")]
        public string FeeDescription { get; set; }
    }

    public class BankBranchInfo
    {
        [Placeholder("Naziv filijale", "Naziv bankarske filijale")]
        public string BranchName { get; set; }
        [Placeholder("Adresa filijale", "Adresa bankarske filijale")]
        public string BranchAddress { get; set; }
        [Placeholder("Grad filijale", "Grad u kojem se nalazi filijala")]
        public string BranchCity { get; set; }
        [Placeholder("Telefon filijale", "Kontakt telefon filijale")]
        public string BranchPhone { get; set; }
        [Placeholder("Šifra filijale", "Jedinstvena šifra filijale")]
        public string BranchCode { get; set; }
    }

    public class ContractInfo
    {
        [Placeholder("Broj ugovora", "Jedinstveni broj ugovora")]
        public string ContractNumber { get; set; }
        [Placeholder("Datum ugovora", "Datum potpisivanja ugovora")]
        public DateTime ContractDate { get; set; }
        [Placeholder("Status ugovora", "Trenutni status ugovora")]
        public string ContractStatus { get; set; }
        [Placeholder("Tip ugovora", "Vrsta bankarskog ugovora")]
        public string ContractType { get; set; }
        [Placeholder("Opis ugovora", "Dodatni opis ugovora")]
        public string ContractDescription { get; set; }
    }

    public class LegalInfo
    {
        [Placeholder("Naziv pravnog lica", "Naziv firme ili pravnog subjekta")]
        public string LegalEntityName { get; set; }
        [Placeholder("ID pravnog lica", "Jedinstveni identifikacioni broj")]
        public string LegalEntityId { get; set; }
        [Placeholder("Adresa pravnog lica", "Adresa pravnog subjekta")]
        public string LegalEntityAddress { get; set; }
        [Placeholder("Telefon pravnog lica", "Kontakt telefon pravnog subjekta")]
        public string LegalEntityPhone { get; set; }
        [Placeholder("Status pravnog lica", "Status pravnog subjekta")]
        public string LegalEntityStatus { get; set; }
    }

    public class NotificationInfo
    {
        [Placeholder("Tip obavijesti", "Vrsta obavijesti (email, SMS...)")]
        public string NotificationType { get; set; }
        [Placeholder("Sadržaj obavijesti", "Tekst obavijesti")]
        public string NotificationContent { get; set; }
        [Placeholder("Datum slanja", "Datum slanja obavijesti")]
        public DateTime NotificationDate { get; set; }
        [Placeholder("Status obavijesti", "Status isporuke obavijesti")]
        public string NotificationStatus { get; set; }
        [Placeholder("Primalac", "Ime i prezime primaoca")]
        public string Recipient { get; set; }
    }

    public class StatementInfo
    {
        [Placeholder("Broj izvoda", "Jedinstveni broj bankarskog izvoda")]
        public string StatementNumber { get; set; }
        [Placeholder("Datum izvoda", "Datum izdavanja izvoda")]
        public DateTime StatementDate { get; set; }
        [Placeholder("Stanje na izvještaju", "Stanje računa na dan izvoda")]
        public decimal StatementBalance { get; set; }
        [Placeholder("Tip izvoda", "Vrsta bankarskog izvoda")]
        public string StatementType { get; set; }
        [Placeholder("Status izvoda", "Status izvoda")]
        public string StatementStatus { get; set; }
    }

    public class TransactionInfo
    {
        [Placeholder("Broj transakcije", "Jedinstveni broj transakcije")]
        public string TransactionNumber { get; set; }
        [Placeholder("Datum transakcije", "Datum izvršenja transakcije")]
        public DateTime TransactionDate { get; set; }
        [Placeholder("Iznos transakcije", "Iznos transakcije")]
        public decimal TransactionAmount { get; set; }
        [Placeholder("Tip transakcije", "Vrsta transakcije (uplata, isplata...)")]
        public string TransactionType { get; set; }
        [Placeholder("Status transakcije", "Status transakcije")]
        public string TransactionStatus { get; set; }
    }

    public class LimitInfo
    {
        [Placeholder("Tip limita", "Vrsta limita (dnevni, mjesečni...)")]
        public string LimitType { get; set; }
        [Placeholder("Iznos limita", "Maksimalni dozvoljeni iznos")]
        public decimal LimitAmount { get; set; }
        [Placeholder("Datum postavljanja", "Datum postavljanja limita")]
        public DateTime LimitDate { get; set; }
        [Placeholder("Status limita", "Status limita")]
        public string LimitStatus { get; set; }
        [Placeholder("Opis limita", "Dodatni opis limita")]
        public string LimitDescription { get; set; }
    }

    public class CurrencyInfo
    {
        [Placeholder("Šifra valute", "ISO šifra valute (npr. BAM, EUR)")]
        public string CurrencyCode { get; set; }
        [Placeholder("Naziv valute", "Naziv valute")]
        public string CurrencyName { get; set; }
        [Placeholder("Simbol valute", "Simbol valute (npr. KM, €)")]
        public string CurrencySymbol { get; set; }
        [Placeholder("Status valute", "Status valute")]
        public string CurrencyStatus { get; set; }
        [Placeholder("Kurs valute", "Trenutni kurs valute")]
        public decimal ExchangeRate { get; set; }
    }

    public class ExchangeRateInfo
    {
        [Placeholder("Valuta", "Valuta za koju se prikazuje kurs")]
        public string Currency { get; set; }
        [Placeholder("Referentna valuta", "Valuta prema kojoj se računa kurs")]
        public string ReferenceCurrency { get; set; }
        [Placeholder("Vrijednost kursa", "Vrijednost kursa")]
        public decimal RateValue { get; set; }
        [Placeholder("Datum kursa", "Datum važenja kursa")]
        public DateTime RateDate { get; set; }
        [Placeholder("Status kursa", "Status kursa")]
        public string RateStatus { get; set; }
    }

    public class InsuranceInfo
    {
        [Placeholder("Naziv osiguranja", "Naziv police osiguranja")]
        public string InsuranceName { get; set; }
        [Placeholder("Broj police", "Jedinstveni broj police osiguranja")]
        public string PolicyNumber { get; set; }
        [Placeholder("Iznos osiguranja", "Ukupan iznos osiguranja")]
        public decimal InsuranceAmount { get; set; }
        [Placeholder("Datum početka", "Datum početka osiguranja")]
        public DateTime StartDate { get; set; }
        [Placeholder("Status osiguranja", "Status police osiguranja")]
        public string InsuranceStatus { get; set; }
    }

    public class TaxInfo
    {
        [Placeholder("Tip poreza", "Vrsta poreza")]
        public string TaxType { get; set; }
        [Placeholder("Iznos poreza", "Ukupan iznos poreza")]
        public decimal TaxAmount { get; set; }
        [Placeholder("Datum obračuna", "Datum obračuna poreza")]
        public DateTime TaxDate { get; set; }
        [Placeholder("Status poreza", "Status obračuna poreza")]
        public string TaxStatus { get; set; }
        [Placeholder("Opis poreza", "Dodatni opis poreza")]
        public string TaxDescription { get; set; }
    }

    public class PenaltyInfo
    {
        [Placeholder("Tip penala", "Vrsta penala")]
        public string PenaltyType { get; set; }
        [Placeholder("Iznos penala", "Ukupan iznos penala")]
        public decimal PenaltyAmount { get; set; }
        [Placeholder("Datum penala", "Datum obračuna penala")]
        public DateTime PenaltyDate { get; set; }
        [Placeholder("Status penala", "Status obračuna penala")]
        public string PenaltyStatus { get; set; }
        [Placeholder("Opis penala", "Dodatni opis penala")]
        public string PenaltyDescription { get; set; }
    }

    public class RepaymentInfo
    {
        [Placeholder("Iznos otplate", "Iznos pojedinačne otplate")]
        public decimal RepaymentAmount { get; set; }
        [Placeholder("Datum otplate", "Datum izvršenja otplate")]
        public DateTime RepaymentDate { get; set; }
        [Placeholder("Status otplate", "Status otplate")]
        public string RepaymentStatus { get; set; }
        [Placeholder("Tip otplate", "Vrsta otplate")]
        public string RepaymentType { get; set; }
        [Placeholder("Opis otplate", "Dodatni opis otplate")]
        public string RepaymentDescription { get; set; }
    }

    public class OverdraftInfo
    {
        [Placeholder("Iznos dozvoljenog minusa", "Maksimalni iznos dozvoljenog minusa")]
        public decimal OverdraftAmount { get; set; }
        [Placeholder("Datum odobrenja", "Datum odobrenja minusa")]
        public DateTime ApprovalDate { get; set; }
        [Placeholder("Status minusa", "Status dozvoljenog minusa")]
        public string OverdraftStatus { get; set; }
        [Placeholder("Tip minusa", "Vrsta dozvoljenog minusa")]
        public string OverdraftType { get; set; }
        [Placeholder("Opis minusa", "Dodatni opis minusa")]
        public string OverdraftDescription { get; set; }
    }

    public class DepositInfo
    {
        [Placeholder("Iznos depozita", "Ukupan iznos depozita")]
        public decimal DepositAmount { get; set; }
        [Placeholder("Datum depozita", "Datum uplate depozita")]
        public DateTime DepositDate { get; set; }
        [Placeholder("Status depozita", "Status depozita")]
        public string DepositStatus { get; set; }
        [Placeholder("Tip depozita", "Vrsta depozita")]
        public string DepositType { get; set; }
        [Placeholder("Opis depozita", "Dodatni opis depozita")]
        public string DepositDescription { get; set; }
    }

    public class SavingsInfo
    {
        [Placeholder("Iznos štednje", "Ukupan iznos štednje")]
        public decimal SavingsAmount { get; set; }
        [Placeholder("Datum štednje", "Datum uplate štednje")]
        public DateTime SavingsDate { get; set; }
        [Placeholder("Status štednje", "Status štednje")]
        public string SavingsStatus { get; set; }
        [Placeholder("Tip štednje", "Vrsta štednje")]
        public string SavingsType { get; set; }
        [Placeholder("Opis štednje", "Dodatni opis štednje")]
        public string SavingsDescription { get; set; }
    }

    public class InvestmentInfo
    {
        [Placeholder("Iznos investicije", "Ukupan iznos investicije")]
        public decimal InvestmentAmount { get; set; }
        [Placeholder("Datum investicije", "Datum ulaganja")]
        public DateTime InvestmentDate { get; set; }
        [Placeholder("Status investicije", "Status investicije")]
        public string InvestmentStatus { get; set; }
        [Placeholder("Tip investicije", "Vrsta investicije")]
        public string InvestmentType { get; set; }
        [Placeholder("Opis investicije", "Dodatni opis investicije")]
        public string InvestmentDescription { get; set; }
    }

    public class PowerOfAttorneyInfo
    {
        [Placeholder("Ime punomoćnika", "Puno ime punomoćnika")]
        public string AttorneyName { get; set; }
        [Placeholder("JMBG punomoćnika", "Jedinstveni matični broj punomoćnika")]
        public string AttorneyId { get; set; }
        [Placeholder("Tip punomoći", "Vrsta punomoći")]
        public string AttorneyType { get; set; }
        [Placeholder("Datum izdavanja", "Datum izdavanja punomoći")]
        public DateTime IssueDate { get; set; }
        [Placeholder("Status punomoći", "Status punomoći")]
        public string AttorneyStatus { get; set; }
    }

    public class DocumentInfo
    {
        [Placeholder("Naziv dokumenta", "Naziv bankarskog dokumenta")]
        public string DocumentName { get; set; }
        [Placeholder("Broj dokumenta", "Jedinstveni broj dokumenta")]
        public string DocumentNumber { get; set; }
        [Placeholder("Datum izdavanja", "Datum izdavanja dokumenta")]
        public DateTime IssueDate { get; set; }
        [Placeholder("Status dokumenta", "Status dokumenta")]
        public string DocumentStatus { get; set; }
        [Placeholder("Tip dokumenta", "Vrsta dokumenta")]
        public string DocumentType { get; set; }
    }

    public class SignatureInfo
    {
        [Placeholder("Ime potpisnika", "Puno ime potpisnika")]
        public string SignerName { get; set; }
        [Placeholder("Datum potpisa", "Datum potpisivanja dokumenta")]
        public DateTime SignatureDate { get; set; }
        [Placeholder("Status potpisa", "Status potpisa")]
        public string SignatureStatus { get; set; }
        [Placeholder("Tip potpisa", "Vrsta potpisa")]
        public string SignatureType { get; set; }
        [Placeholder("Opis potpisa", "Dodatni opis potpisa")]
        public string SignatureDescription { get; set; }
    }

    public class ApprovalInfo
    {
        [Placeholder("Ime odobravaoca", "Puno ime osobe koja odobrava")]
        public string ApproverName { get; set; }
        [Placeholder("Datum odobrenja", "Datum odobravanja")]
        public DateTime ApprovalDate { get; set; }
        [Placeholder("Status odobrenja", "Status odobrenja")]
        public string ApprovalStatus { get; set; }
        [Placeholder("Tip odobrenja", "Vrsta odobrenja")]
        public string ApprovalType { get; set; }
        [Placeholder("Opis odobrenja", "Dodatni opis odobrenja")]
        public string ApprovalDescription { get; set; }
    }

    public class DisbursementInfo
    {
        [Placeholder("Iznos isplate", "Ukupan iznos isplate")]
        public decimal DisbursementAmount { get; set; }
        [Placeholder("Datum isplate", "Datum izvršenja isplate")]
        public DateTime DisbursementDate { get; set; }
        [Placeholder("Status isplate", "Status isplate")]
        public string DisbursementStatus { get; set; }
        [Placeholder("Tip isplate", "Vrsta isplate")]
        public string DisbursementType { get; set; }
        [Placeholder("Opis isplate", "Dodatni opis isplate")]
        public string DisbursementDescription { get; set; }
    }

    public class ClosureInfo
    {
        [Placeholder("Datum zatvaranja", "Datum zatvaranja računa/ugovora")]
        public DateTime ClosureDate { get; set; }
        [Placeholder("Status zatvaranja", "Status zatvaranja")]
        public string ClosureStatus { get; set; }
        [Placeholder("Tip zatvaranja", "Vrsta zatvaranja")]
        public string ClosureType { get; set; }
        [Placeholder("Opis zatvaranja", "Dodatni opis zatvaranja")]
        public string ClosureDescription { get; set; }
        [Placeholder("Razlog zatvaranja", "Razlog zatvaranja")]
        public string ClosureReason { get; set; }
    }

    public class AmendmentInfo
    {
        [Placeholder("Broj aneksa", "Jedinstveni broj aneksa")]
        public string AmendmentNumber { get; set; }
        [Placeholder("Datum aneksa", "Datum potpisivanja aneksa")]
        public DateTime AmendmentDate { get; set; }
        [Placeholder("Status aneksa", "Status aneksa")]
        public string AmendmentStatus { get; set; }
        [Placeholder("Tip aneksa", "Vrsta aneksa")]
        public string AmendmentType { get; set; }
        [Placeholder("Opis aneksa", "Dodatni opis aneksa")]
        public string AmendmentDescription { get; set; }
    }

    public class ConsentInfo
    {
        [Placeholder("Tip saglasnosti", "Vrsta saglasnosti")]
        public string ConsentType { get; set; }
        [Placeholder("Datum saglasnosti", "Datum davanja saglasnosti")]
        public DateTime ConsentDate { get; set; }
        [Placeholder("Status saglasnosti", "Status saglasnosti")]
        public string ConsentStatus { get; set; }
        [Placeholder("Opis saglasnosti", "Dodatni opis saglasnosti")]
        public string ConsentDescription { get; set; }
        [Placeholder("Ime davaoca", "Puno ime davaoca saglasnosti")]
        public string ConsentGiver { get; set; }
    }

    public class RiskAssessmentInfo
    {
        [Placeholder("Tip procjene rizika", "Vrsta procjene rizika")]
        public string RiskType { get; set; }
        [Placeholder("Datum procjene", "Datum procjene rizika")]
        public DateTime RiskDate { get; set; }
        [Placeholder("Status procjene", "Status procjene rizika")]
        public string RiskStatus { get; set; }
        [Placeholder("Opis rizika", "Dodatni opis rizika")]
        public string RiskDescription { get; set; }
        [Placeholder("Procjenitelj", "Ime osobe koja je izvršila procjenu")]
        public string Assessor { get; set; }
    }

    public class ComplianceInfo
    {
        [Placeholder("Tip usklađenosti", "Vrsta regulatorne usklađenosti")]
        public string ComplianceType { get; set; }
        [Placeholder("Datum usklađenosti", "Datum provjere usklađenosti")]
        public DateTime ComplianceDate { get; set; }
        [Placeholder("Status usklađenosti", "Status usklađenosti")]
        public string ComplianceStatus { get; set; }
        [Placeholder("Opis usklađenosti", "Dodatni opis usklađenosti")]
        public string ComplianceDescription { get; set; }
        [Placeholder("Ime provjeravaoca", "Puno ime osobe koja je izvršila provjeru")]
        public string ComplianceOfficer { get; set; }
    }

    public class AuditInfo
    {
        [Placeholder("Tip revizije", "Vrsta revizije")]
        public string AuditType { get; set; }
        [Placeholder("Datum revizije", "Datum izvršenja revizije")]
        public DateTime AuditDate { get; set; }
        [Placeholder("Status revizije", "Status revizije")]
        public string AuditStatus { get; set; }
        [Placeholder("Opis revizije", "Dodatni opis revizije")]
        public string AuditDescription { get; set; }
        [Placeholder("Ime revizora", "Puno ime revizora")]
        public string Auditor { get; set; }
    }

    public class NotificationSettings
    {
        [Placeholder("Email obavijesti", "Status email obavijesti")]
        public bool EmailEnabled { get; set; }
        [Placeholder("SMS obavijesti", "Status SMS obavijesti")]
        public bool SmsEnabled { get; set; }
        [Placeholder("Push obavijesti", "Status push obavijesti")]
        public bool PushEnabled { get; set; }
        [Placeholder("Jezik obavijesti", "Jezik na kojem se šalju obavijesti")]
        public string NotificationLanguage { get; set; }
        [Placeholder("Vrijeme slanja", "Preferirano vrijeme slanja obavijesti")]
        public string NotificationTime { get; set; }
    }
}
public enum EmploymentStatusEnum
{
    Zaposlen,
    Nezaposlen,
    Student,
    Penzioner,
    Ostalo
}