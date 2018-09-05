export class UploadFile
{
    UploadFileId: number;
    Name: string;
        ApplicationId: number;
        ComponentId: number;
        FileInputPath : string;
        InputFileMask : string;
        IsArchiveOutputRequired : boolean;
        ArchiveOutputPath : string;
        ArchiveFileTransferSettingId : number;
        IsMoveFileRequired : boolean;
        MoveFileExpression : string;
        ArchiveFileExpression : string;
        MoveFilePath : string;
        MoveFileTransferSettingId : number;
        Description : string;         
    Status: boolean;
}