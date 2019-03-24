export class EmailTemplate
{
    EmailTemplateId: number;
    EmailFromSmtpId: number;
    EmailToIds: string;
    Subject: string;
    EmailLevelId : number;
    EmailCcIds : string;
    Body : string;
    EmailToken : string;
    ClientId : number;
    ApplicationId : number;
    ApplicationComponentId : number;
    TimeInterval: number;
    Status:boolean
}